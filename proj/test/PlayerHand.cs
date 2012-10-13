using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    class PlayerHand : BlackjackHand {
        private double myBet;
        private bool hasSplit;

        public bool HasSplit {
            get { return hasSplit; }
        }
        private bool hasDoubledDown;

        public bool HasDoubledDown {
            get { return hasDoubledDown; }
        }
        private PlayerHand psplit;

        internal PlayerHand SplitHand {
            get { return psplit; }
        }

        public double Bet {
            get { return myBet; }
            set { myBet = value; }
        }

        public bool CanSplit {
            get {
                return this.Count == 2 && this.cards[0].Equals(this.cards[1]);
            }
        }

        public PlayerHand(Shoe shoe) : base(shoe) { }
        public PlayerHand(Shoe ParentShoe, bool p) : base(ParentShoe, p) { }

        public PlayerHand Split() {
            // TODO: Does the split need to be only on two cards?
            PlayerHand r = new PlayerHand(ParentShoe, false);
            r.cards.Add(this.Discard(0));
            return r;
        }

        public override string ToString() {
            List<string> s = new List<string>();
            foreach (Card c in cards) {
                s.Add(c.ToString());
            }

            return String.Format("{0} (sum:{2}) [{1}]", cards.Count, String.Join<string>(", ", s.ToArray()), Sum);
        }

        public override bool doTurn(BlackjackAction c) {
            //if (hasDoubledDown) // TODO: Is this functionality required anymore?
            //    c = BlackjackAction.DoubleDown;
            //else
            //    c = displayMenu();

            switch (c) {
                case BlackjackAction.Hit:
                    Draw();
                    if (hasSplit) {
                        psplit.Draw();
                    }
                    //dealer.doTurn();
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
                        Draw();
                        Bet *= 2;
                        hasDoubledDown = true;
                    }
                    break;
                case BlackjackAction.EndGame:
                    Console.Clear();
                    Console.WriteLine("Peace bro");
                    Console.ReadKey();
                    Environment.Exit(0);
                    return true;
                default:
                    throw new ArgumentOutOfRangeException("What?");
            }
            return false;
        }
        public override void PutCardsBack() {
            if (psplit != null) {
                psplit.DiscardAll();
            }
            DiscardAll();
        }
    }
}
