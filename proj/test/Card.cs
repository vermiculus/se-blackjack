using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    class Card : IComparable<Card> {
        Suit mySuite;
        Rank myRank;
        Deck parentDeck; // TODO: Discard back into parent deck


        public Rank Rank {
            get {
                return myRank;
            }
            set {
                myRank = value;
            }
        }
        public Suit Suit {
            get {
                return mySuite;
            }
            set {
                mySuite = value;
            }
        }

        public Card(Rank rank, Suit suit) : this(rank, suit, null) { }

        public Card(Rank rank, Suit suit, Deck momma) {
            this.Rank = rank;
            this.Suit = suit;
            parentDeck = momma;
        }

        /// <summary>
        /// Returns a String representation of the Card
        /// </summary>
        public override string ToString() {
            var r = (char)Rank + "" + (char)Suit;
            return r;
        }

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

        public void Replace() {
            this.parentDeck.ReplaceCard(this);
        }
    }
}
