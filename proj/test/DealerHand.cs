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
    }
}
