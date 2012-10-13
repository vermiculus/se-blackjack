using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    class DealerHand : BlackjackHand {
        public DealerHand(Shoe shoe) : base(shoe) { }

        public override string ToString() {
            return String.Format("{0} [{1}{2}]", cards.Count, cards[0].ToString(), cards.Count > 1 ? "..." : "");
        }

        public override bool doTurn(BlackjackAction action) {
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

        public string ToRevealingString() {
            List<string> s = new List<string>();
            foreach (Card c in cards) {
                s.Add(c.ToString());
            }

            return String.Format("{0} (sum:{2}) [{1}]", cards.Count, String.Join<string>(", ", s.ToArray()), Sum);
        }

        public override void PutCardsBack() {
            DiscardAll();
        }
    }
}
