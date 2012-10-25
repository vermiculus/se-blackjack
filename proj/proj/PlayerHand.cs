using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    class PlayerHand : BlackjackHand {
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

        private bool hasSplit;

        /// <summary>
        /// Returns true if this hand has split
        /// </summary>
        public bool HasSplit {
            get { return hasSplit; }
        }

        private PlayerHand psplit;

        /// <summary>
        /// The hand that results from a split
        /// </summary>
        internal PlayerHand SplitHand {
            get { return psplit; }
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
                return this.Count == 2 && this.cards[0].Rank == this.cards[1].Rank && !HasSplit && Cash >= Bet;
                // return this.Count == 2 && cardValue(this.cards[0]) == cardValue(this.cards[1]) && !HasSplit && Cash >= Bet;
            }
        }

        public PlayerHand(CardCollection source, CardCollection discard, uint cash = DEFAULT_CASH)
            : base(source, discard) {
            this.myCash = cash;
        }

        public PlayerHand Split() {
            // TODO: Does the split need to be only on two cards?
            PlayerHand r = new PlayerHand(SourceCollection, DiscardCollection);
            r.DiscardAll();
            r.cards.Add(this.Discard(0));
            return r;
        }

        public override string ToString() {
            List<string> s1 = new List<string>();
            List<string> s2 = new List<string>();
            foreach (Card c in cards) {
                s1.Add(c.ToString());
            }
            if (psplit != null) {
                foreach (Card c in psplit.cards) {
                    s2.Add(c.ToString());
                }
            }
            if (psplit == null) {
                return String.Format("{0} (sum:{2}) [{1}]", cards.Count, String.Join<string>(", ", s1.ToArray()), Sum);
            } else {
                return String.Format("{0} (sum:{2}) [[{1}],[{3}]]", cards.Count, String.Join<string>(", ", s1.ToArray()), Sum, String.Join<string>(", ", s2.ToArray()));
            }
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
                return psplit.doTurn(c);
            }

            switch (c) {
                case BlackjackAction.Hit:
                    Draw();
                    if (hasSplit) {
                        psplit.Draw();
                    }
                    break;
                case BlackjackAction.Stand:
                    // taken care of in Game (Standing is a complete *lack* of action of the Player
                    break;
                case BlackjackAction.Split:
                    if (Count == 2 && !hasSplit && CanSplit) {
                        psplit = Split();
                        hasSplit = true;
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
            if (psplit != null) {
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
        internal void makeTurns(Card dealerFaceUpCard) {
            if (CanSplit) {
                
            }
            Console.WriteLine("\n Cash: {0,4:N0}  Bet: {1,3:N0}\n", Cash, Bet);
            Console.WriteLine("  Dealer's Hand: {0}", dealerFaceUpCard);
            Console.WriteLine("      Your Hand: {0}", this);
        }

        private BlackjackAction displayMenu(ActiveHand a = ActiveHand.Normal) {
            Console.Clear();
            printHands();
            Console.WriteLine("\n\n     What would you like to do? {0}\n", HasSplit ? "" : a == ActiveHand.Normal ? "(Hand 1)" : "(Hand 2)");
            Console.WriteLine(" [1] Hit");
            Console.WriteLine(" [2] Stand");
            Console.WriteLine(" [3] Split");
            Console.WriteLine(" [0] Quit Game");
            //TODO: Allow a doubling factor of 1.0 to 2.0 [what did I mean by this? - from old code]

            ConsoleKeyInfo k = Console.ReadKey(true);
            if (!Char.IsDigit(k.KeyChar)) {
                Console.WriteLine("  Invalid option. Choose 0-3.");

                return displayMenu();
            }
            int t = Int32.Parse("" + k.KeyChar);
            if (t < 0 || t > 3) {
                Console.WriteLine("  Invalid option. Choose 0-3.");
                return displayMenu();
            } else {
                switch (t) {
                    case 0:
                        return BlackjackAction.EndGame;
                    case 1:
                        return BlackjackAction.Hit;
                    case 2:
                        return BlackjackAction.Stand;
                    case 3:
                        return BlackjackAction.Split;
                    default:
                        throw new ArgumentOutOfRangeException("Wha happun?");
                };
            }
        }
    }
}
