using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    /// <summary>
    /// Represents a standard playing card
    /// </summary>
    class Card : IComparable<Card> {
        /// <summary>
        /// The suit of this Card
        /// </summary>
        Suit mySuite;
        /// <summary>
        /// The rank of this Card
        /// </summary>
        Rank myRank;
        /// <summary>
        /// The CardSource this Card should be discarded to
        /// </summary>
        CardSource discard;


        /// <summary>
        /// The rank of this Card
        /// </summary>
        public Rank Rank {
            get {
                return myRank;
            }
            set {
                myRank = value;
            }
        }
        /// <summary>
        /// The suit of this Card
        /// </summary>
        public Suit Suit {
            get {
                return mySuite;
            }
            set {
                mySuite = value;
            }
        }

        /// <param name="rank">The rank of the new Card</param>
        /// <param name="suit">The suit of the new Card</param>
        /// <param name="discard">The CardSource this Card should discard to</param>
        public Card(Rank rank, Suit suit, CardSource discard = null) {
            this.Rank = rank;
            this.Suit = suit;
            this.discard = discard;
        }

        /// <summary>
        /// Returns a String representation of the Card
        /// </summary>
        public override string ToString() {
            var r = (char)Rank + "" + (char)Suit;
            return r;
        }

        /// <summary>
        /// Orders by suit and then by rank, ace low.
        /// </summary>
        /// <param name="c">A Card to compare to</param>
        public int CompareTo(Card c) {
            if (c.Suit > this.Suit)
                return 1;
            else if (c.Suit < this.Suit)
                return -1;
            else if (c.Rank > this.Rank)
                return 1;
            else if (c.Rank < this.Rank)
                return -1;
            else return 0;
        }

        /// <summary>
        /// Returns true if two cards are of equal rank and suit. Note that they need not be from the same deck.
        /// </summary>
        /// <param name="obj">An object to test equality on</param>
        public override bool Equals(Object obj) {
            if (obj.GetType() != this.GetType()) {
                return false;
            } else {
                return this.CompareTo((Card)obj) == 0;
            }
        }

        public override int GetHashCode() {
            return (int)Rank * 10 + (int)Suit;
        }

        /// <summary>
        /// Places the card back in the deck from which it was drawn.
        /// </summary>
        /// <!--This method is slated for refactoring per issue #3-->
        public void Replace() {
            this.discard.ReplaceCard(this);
        }
    }
}
