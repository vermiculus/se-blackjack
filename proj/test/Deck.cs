using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    /// <summary>
    /// A standard collection of Cards
    /// </summary>
    class Deck {
        /// <summary>
        /// The actual deck - a collection of Cards
        /// </summary>
        List<Card> cards;
        /// <summary>
        /// Creates a new Deck and populates it with all 52 standard cards
        /// </summary>
        public Deck() {
            cards = new List<Card>();
            foreach (Suit s in Enum.GetValues(typeof(Suit))) {
                foreach (Rank r in Enum.GetValues(typeof(Rank))) {
                    cards.Add(new Card(r, s, this));
                }
            }
        }

        /// <summary>
        /// Returns true if there are no cards left in the Deck, false otherwise
        /// </summary>
        public bool IsEmpty {
            get {
                return Count == 0;
            }
        }

        /// <summary>
        /// The number of cards currently in the Deck
        /// </summary>
        public uint Count {
            get {
                return (uint)cards.Count;
            }
        }

        /// <summary>
        /// Returns a random card from this Deck, removing it internally
        /// </summary>
        public Card Draw() {
            if (IsEmpty)
                return null;
            Card r = cards[(new Random()).Next(cards.Count)];
            cards.Remove(r);
            return r;
        }

        /// <summary>
        /// Returns a String representation of this Deck
        /// </summary>
        public override string ToString() {
            return cards.Count + " " + cards.ToString();
        }

        /// <summary>
        /// Places the given card back in this Deck
        /// </summary>
        /// <param name="c">The card to replace</param>
        public void ReplaceCard(Card c) {
            if (!cards.Contains(c)) {
                cards.Add(c);
            }
        }
    }
}
