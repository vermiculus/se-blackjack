using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack {
    class DealerHand : BlackjackHand {
        /// <summary>
        /// Creates a Dealer Hand associated with the specified shoe
        /// </summary>
        /// <param name="shoe">the Shoe to associate with</param>
        public DealerHand(CardCollection source, CardCollection discard) : base(source, discard) { }

        /// <summary>
        /// Returns a String representation of this DealerHand where only the Top card is visible
        /// </summary>
        public override string ToString() {
            return String.Format("{0} [{1}{2}]", Cards.Count, Cards[0].ToString(), Cards.Count > 1 ? "..." : "");
        }

        /// <summary>
        /// Completes one turn of a Dealer according to appropriate logic. The only valid BlackjackAction is None.
        /// </summary>
        public void doTurn() {
            // TODO: verify
            bool again = true;
            while (again) {
                if (Sum < 17 || (NumberOfAces > 0 && Sum <= 17)) {
                    Draw();
                } else {
                    again = false;
                }
            }
        }

        /// <summary>
        /// Returns a String representation of this DealerHand where every card is visible
        /// </summary>
        public string ToRevealingString() {
            List<string> s = new List<string>();
            foreach (Card c in Cards) {
                s.Add(c.ToString());
            }

            return String.Format("{0} (sum:{2}) [{1}]", Cards.Count, String.Join<string>(", ", s.ToArray()), Sum);
        }

        /// <summary>
        /// Discards all cards
        /// </summary>
        public override void PutCardsBack() {
            DiscardAll();
        }
    }
}
