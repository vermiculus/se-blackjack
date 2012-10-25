using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack {
    class CardCollection {
        /// <summary>
        /// The collection Cards that make up the Shoe
        /// </summary>
        protected List<Card> cards;

        private uint original_deck_size;

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
        /// Creates a Shoe containing the specified number of Decks
        /// </summary>
        /// <param name="count">The number of Decks to create</param>
        public CardCollection(uint deckCount = 0, bool makeCards = true) {
            original_deck_size = deckCount;
            cards = new List<Card>();
            if (makeCards) {
                for (int i = 0; i < deckCount; i++) { // for each deck,
                    foreach (Suit s in Enum.GetValues(typeof(Suit))) { // for each possible Suit
                        foreach (Rank r in Enum.GetValues(typeof(Rank))) { // for each possible Rank
                            cards.Add(new Card(r, s));
                        }
                    }
                }
                Sort();
            }
        }

        /// <summary>
        /// The total number of Decks that make up the Shoe
        /// </summary>
        public uint DeckCount {
            get { return original_deck_size; }
        }

        internal bool add(Card c) {
            if (count(c) < original_deck_size) {
                cards.Add(c);
                return true;
            }
            return false;
        }

        private uint count(Card c) {
            uint sum = 0;
            foreach (Card card in cards) {
                if (c.Equals(card)) {
                    sum++;
                }
            }
            return sum;
        }

        public void Sort() {
            cards.Sort();
        }

        /// <summary>
        /// Refill some other CardCollection with the cards from this CardCollection
        /// </summary>
        /// <param name="destination"></param>
        internal void Refill(CardCollection destination) {
            destination.cards.InsertRange(0, cards);
            cards.Clear();
        }
    }
}
