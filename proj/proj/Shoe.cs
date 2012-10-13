using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    /// <summary>
    /// A collection of Decks
    /// </summary>
    class Shoe {
        /// <summary>
        /// The collection Decks that make up the Shoe
        /// </summary>
        private List<Deck> decks;

        /// <summary>
        /// Creates a Shoe containing the specified number of Decks
        /// </summary>
        /// <param name="count">The number of Decks to create</param>
        public Shoe(uint count) {
            decks = new List<Deck>();
            for (int i = 0; i < count; i++) {
                decks.Add(new Deck());
            }
        }

        /// <summary>
        /// Creates a Shoe containg a single given Deck
        /// </summary>
        /// <param name="deck">The deck this Shoe is to contain</param>
        public Shoe(Deck deck) {
            this.decks = new List<Deck>();
            this.decks.Add(deck);
        }

        public Card Draw() {
            Random rn = new Random();
            int i = rn.Next((int)DeckCount);
            while (decks[i].IsEmpty) {
                i = rn.Next((int)DeckCount);
            }
            return decks[i].Draw();
        }

        /// <summary>
        /// True if there are no cards left in the Shoe (CardCount==0), false otherwise
        /// </summary>
        public bool isEmpty {
            get {
                bool r = true;
                foreach (Deck deck in decks) {
                    r &= deck.IsEmpty;
                }
                return r;
            }
        }

        /// <summary>
        /// The total number of Decks that make up the Shoe
        /// </summary>
        public uint DeckCount {
            get {
                return (uint)decks.Count;
            }
        }

        /// <summary>
        /// The total number of Cards currently available in the Shoe
        /// </summary>
        public uint CardCount {
            get {
                uint r = 0;
                foreach (Deck d in decks) {
                    r += (uint)d.Count;
                }
                return r;
            }
        }
    }
}
