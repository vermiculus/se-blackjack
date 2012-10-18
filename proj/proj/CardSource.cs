using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    class CardSource {
        /// <summary>
        /// The collection Cards that make up the Shoe
        /// </summary>
        protected List<Card> cards;
        public uint Count {
            get { return (uint)cards.Count; }
        }
        public bool IsEmpty {
            get { return Count == 0; }
        }

        /// <summary>
        /// Returns a random card from this Source, removing it internally
        /// </summary>
        public Card Draw() {
            if (IsEmpty)
                return null;
            Card r = cards[(new Random()).Next(cards.Count)];
            cards.Remove(r);
            return r;
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
        private uint original_deck_size;

        /// <summary>
        /// Creates a Shoe containing the specified number of Decks
        /// </summary>
        /// <param name="count">The number of Decks to create</param>
        public CardSource(uint deckCount = 0) {
            original_deck_size = deckCount;
            cards = new List<Card>();
            for (int i = 0; i < deckCount; i++) {
                foreach (Suit s in Enum.GetValues(typeof(Suit))) {
                    foreach (Rank r in Enum.GetValues(typeof(Rank))) {
                        cards.Add(new Card(r, s, this));
                    }
                }
            }
        }

        /// <summary>
        /// The total number of Decks that make up the Shoe
        /// </summary>
        public uint DeckCount {
            get { return original_deck_size; }
        }
    }
}
