using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackerJack {
    public class PlayerHand : BlackjackHand {
        private double myBet;
        public override string ToString() {
            List<string> s = new List<string>();
            foreach (Card c in cards) {
                s.Add(c.ToString());
            }

            return String.Format("{0} ({2}) [{1}]", cards.Count, String.Join<string>(", ", s.ToArray()), Sum);
        }

        public BlackjackHand Split() {
            // TODO: Does the split need to be only on two cards?
            BlackjackHand r = new BlackjackHand(ParentShoe, false);
            r.cards.Add(this.Discard(0));
            return r;
        }
    }
}
