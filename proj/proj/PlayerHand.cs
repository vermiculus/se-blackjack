using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    class PlayerHand : BlackjackHand {
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
        private bool hasDoubledDown;

        /// <summary>
        /// Returns true if this hand has doubled down
        /// </summary>
        public bool HasDoubledDown {
            get { return hasDoubledDown; }
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
                return this.Count == 2 && this.cards[0].Rank == this.cards[1].Rank;
            }
        }

        public PlayerHand(Shoe shoe, uint cash = DEFAULT_CASH) : base(shoe, 2) {
            this.myCash = cash;
        }

        public PlayerHand Split() {
            // TODO: Does the split need to be only on two cards?
            PlayerHand r = new PlayerHand(ParentShoe);
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

        public override bool doTurn(BlackjackAction c) {
            //if (hasDoubledDown) // TODO: Is this functionality required anymore? At any rate, it should be moved to Game. The menu should have to be displayed.
            //    c = BlackjackAction.DoubleDown;

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
                case BlackjackAction.DoubleDown:
                    if (Count == 2) {
                        Draw(); // TODO: really? does the player actually draw?
                        myBet *= 2;
                        hasDoubledDown = true;
                    }
                    break;
                case BlackjackAction.EndGame:
                    // also taken care of in Game - this is a high level action.
                    return true;
                case BlackjackAction.None:
                    throw new InvalidOperationException("'None' is not an acceptable action for this class.");
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

        // TODO: When the hand has been split on two aces, the aces do not 'buckle down' if necessary.
        // Adding 'new' tells the compiler that I know this property in the base class is being hidden.
        public new uint Sum {
            get {
                if (psplit == null) {
                    return base.Sum;
                } else {
                    return base.Sum + psplit.Sum;
                }
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
    }
}
