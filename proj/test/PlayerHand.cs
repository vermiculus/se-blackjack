using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    class PlayerHand : BlackjackHand {
        private double myBet;

        public double Bet {
            get { return myBet; }
            set { myBet = value; }
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
    }
}
