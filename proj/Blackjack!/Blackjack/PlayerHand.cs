using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack {
    class PlayerHand : BlackjackHand {

        /// <summary>
        /// Denotes which hand the player is currently making moves for.
        /// </summary>
        private enum ActiveHand {
            Normal,
            Split
        }

        private uint myBet;
        private uint myCash;
        public const int DEFAULT_CASH = 500;

        public uint Cash {
            get { return myCash; }
        }
        
        private PlayerHand psplit;

        /// <summary>
        /// The hand that results from a split
        /// </summary>
        internal PlayerHand SplitHand {
            get { return psplit; }
        }

        public bool HasSplit
        {
            get { return psplit != null; }
        }

        /// <summary>
        /// The Bet that rides on this specific Hand
        /// </summary>
        /// <remarks>Note that the SplitHand has its own separate bet.</remarks>
        public uint Bet {
            get { return myBet; }
            set { myBet = value; }
        }

        /// <summary>
        /// Returns true if this hand could split
        /// </summary>
        public bool CanSplit {
            get {
                return Count == 2 && Cards[0].Rank == Cards[1].Rank && !HasSplit && Cash >= Bet;
            }
        }

        public PlayerHand(CardCollection source, CardCollection discard, uint cash = DEFAULT_CASH)
            : base(source, discard) {
            this.myCash = cash;
        }

        public void Split() {
            psplit = new PlayerHand(SourceCollection, DiscardCollection);
            psplit.Cards.Add(this.Discard(0));
            psplit.psplit = this;
            psplit.myBet = myBet;
            myCash -= myBet;
        }

        public override string ToString() {
            List<string> s1 = new List<string>();
            foreach (Card c in Cards) {
                s1.Add(c.ToString());
            }
            return String.Format("{0} (sum:{2}) [{1}]", Cards.Count, String.Join<string>(", ", s1.ToArray()), Sum);
        }

        /// <summary>
        /// Performs a given turn
        /// </summary>
        /// <param name="c">The action that the player chose</param>
        /// <param name="onSplit">If the move is for the second hand in a split, set this to true. Otherwise, the default is false.</param>
        /// <returns>Returns false if the game should continue, true if the game should end.</returns>
        //TODO: the whole returning thing makes no sense. See below.
        // If the player chose to end the game, we should totally not be going into this method just to realize they chose that. We should end it there, in Game.
        public bool doTurn(BlackjackAction c, bool onSplit = false) {
            if (onSplit) {
                return SplitHand.doTurn(c);
            }

            switch (c) {
                case BlackjackAction.Hit:
                    Draw();
                    break;
                case BlackjackAction.Stand:
                    // taken care of in Game (Standing is a complete *lack* of action of the Player
                    break;
                case BlackjackAction.Split:
                    if (Count == 2 && CanSplit) {
                        Split();
                    }
                    break;
                case BlackjackAction.EndGame:
                    // also taken care of in Game - this is a high level action.
                    return true;
                default:
                    throw new ArgumentOutOfRangeException("What?");
            }
            return false;
        }

        public override void PutCardsBack() {
            if (HasSplit) {
                psplit.DiscardAll();
                psplit = null;
            }
            DiscardAll();
        }

        public void doBet(WinLoss w) {
            switch (w) {
                case WinLoss.Dealer:
                    myBet = 0;
                    break;
                case WinLoss.Player:
                    myCash += Bet * 2;
                    myBet = 0;
                    break;
                case WinLoss.Push:
                    myCash += Bet;
                    myBet = 0;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets this hand's bet and decreases cash accordingly
        /// </summary>
        /// <param name="bet"></param>
        public void makeBet(uint bet) {
            myBet = bet;
            myCash -= myBet;
        }

        /// <summary>
        /// Undos the work done by makeBet and places a different bet.
        /// </summary>
        /// <param name="bet"></param>
        public void remakeBet(uint bet) {
            myCash += myBet;
            makeBet(bet);
        }

        /// <summary>
        /// Determines and executes the player's choice of turns (Console version)
        /// </summary>
        /// <param name="dealerFaceUpCard">Displayed to the player</param>
        internal void makeTurns(Card dealerFaceUpCard, Action onquit) {
            // Sets up a small internal function
            Action<PlayerHand> printHands = (activehand) => {
                Console.WriteLine("\n Cash: {0,4:N0}  Bet: {1,3:N0}\n", Cash, Bet);
                Console.WriteLine("  Dealer's Hand: {0}", dealerFaceUpCard);
                Console.WriteLine("      Your Hand: {0}", activehand);
            };

            Action<PlayerHand> doMoves = (p) =>
            {
                bool again = true;
                while (again) {
                    if (p.Sum >= 21) {
                        again = false;
                    } else {
                        Console.Clear();
                        printHands(p);
                        p.displayMenu();
                        BlackjackAction c;
                        switch (c = p.getChoice()) {
                            case BlackjackAction.Hit:
                                p.Draw();
                                break;
                            case BlackjackAction.Split:
                                p.Split();
                                break;
                            case BlackjackAction.Stand:
                                again = false;
                                break;
                            case BlackjackAction.EndGame:
                                onquit();
                                again = false;
                                break;
                            default:
                                throw new InvalidOperationException(String.Format("Unable to act on BlackjackAction {0} (numeric {1}).", c, (int)c));
                        }
                    }
                }
            };

            doMoves(this);
            if (HasSplit) {
                doMoves(SplitHand);
            }
        }

        private void displayMenu(ActiveHand a = ActiveHand.Normal) {
            Console.WriteLine("\n\n"+
                              "     What would you like to do?{0}\n", HasSplit ? "" 
                                                                               : a == ActiveHand.Normal ? " (Hand 1)" 
                                                                                                        : " (Hand 2)");
            Console.WriteLine(" [1] Hit");
            Console.WriteLine(" [2] Stand");
            if (CanSplit) {
                Console.WriteLine(" [3] Split");
            }
            Console.WriteLine(" [0] Quit Game");
        }

        /// <summary>
        /// Waits for a keystroke from the user and returns the appropriate BlackjackAction.
        /// This function will continue until the user has input a valid choice.
        /// </summary>
        /// <returns></returns>
        private BlackjackAction getChoice() {
            ConsoleKeyInfo k = Console.ReadKey(true);
            int choice = -1;

            // This one is a little complicated, so let me explain:
            // Try to parse the key the user just pressed as an integer. If you're successful, put the result in 'choice' and return 'true'.
            // (If it is unsuccessul, short-circuiting will prevent the predicate from evaluating)
            // Then, make sure their choice was a positive number.
            // If they can split, allow that choice (menu item 3).
            // Otherwise, disallow it.
            // In other words, if the user can split, they should be able to select 'split' - but not otherwise.
            while (!Int32.TryParse(k.KeyChar.ToString(), out choice) || choice < 0 || choice > (CanSplit ? 3 : 2)) {
                k = Console.ReadKey(true);
            }
            switch (choice) {
                case 0:
                    return BlackjackAction.EndGame;
                case 1:
                    return BlackjackAction.Hit;
                case 2:
                    return BlackjackAction.Stand;
                case 3:
                    return BlackjackAction.Split;
                default:
                    throw new ArgumentOutOfRangeException(String.Format("The user pressed an unrecognized key and the error checking failed to recognize: {0}", k));
            };
        }
    }
}
