using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    class DealerHand : BlackjackHand {
        /// <summary>
        /// Creates a Dealer Hand associated with the specified shoe
        /// </summary>
        /// <param name="shoe">the Shoe to associate with</param>
        public DealerHand(Shoe shoe) : base(shoe) { }

        /// <summary>
        /// Returns a String representation of this DealerHand where only the Top card is visible
        /// </summary>
        public override string ToString() {
            return this.ToRevealingString();// return String.Format("{0} [{1}{2}]", cards.Count, cards[0].ToString(), cards.Count > 1 ? "..." : "");
        }

        /// <summary>
        /// Completes one turn of a Dealer according to appropriate logic. The only valid BlackjackAction is None.
        /// </summary>
        public override bool doTurn(BlackjackAction action = BlackjackAction.None) {
            switch (action) {
                case BlackjackAction.Hit:
                case BlackjackAction.Stand:
                case BlackjackAction.Split:
                case BlackjackAction.DoubleDown:
                case BlackjackAction.EndGame:
                    throw new InvalidOperationException("Invalid action. Only acceptable action is Action.None.");
                case BlackjackAction.None:
                    // TODO: verify
                    if (Sum < 17 || (NumberOfAces > 0 && Sum <= 17)) {
                        Draw();
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// Returns a String representation of this DealerHand where every card is visible
        /// </summary>
        public string ToRevealingString() {
            List<string> s = new List<string>();
            foreach (Card c in cards) {
                s.Add(c.ToString());
            }

            return String.Format("{0} (sum:{2}) [{1}]", cards.Count, String.Join<string>(", ", s.ToArray()), Sum);
        }

        /// <summary>
        /// Discards all cards
        /// </summary>
        public override void PutCardsBack() {
            DiscardAll();
        }
    }
}
