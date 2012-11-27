using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack {
    /// <summary>
    /// An ordered collection of Cards
    /// </summary>
    public class Hand {
        /// <summary>
        /// The source this Hand draws from
        /// </summary>
        private CardCollection source;

        public CardCollection SourceCollection {
            get { return source; }
        }

        /// <summary>
        /// The shoe this Hand discards to
        /// </summary>
        private CardCollection discard;

        public CardCollection DiscardCollection {
            get { return discard; }
        }

        /// <summary>
        /// The collection of Cards that make up this Hand
        /// </summary>
        private List<Card> cards;

        public List<Card> Cards
        {
            get { return cards; }
        }

        /// <summary>
        /// Creates a new Hand from a Shoe of a specifies size
        /// </summary>
        /// <param name="size">The size of the new Hand</param>
        public Hand(CardCollection source, CardCollection discard = null) {
            if (discard == null) {
                discard = source;
            }
            this.cards = new List<Card>();
            this.source = source;
            this.discard = discard;
        }

        /// <summary>
        /// Draws the specified number of cards from the parent shoe
        /// </summary>
        /// <param name="size">The number of cards to draw, defaulting to one</param>
        public void Draw(uint size = 1) {
            for (int i = 0; i < size; i++) {
                if (source.IsEmpty) {
                    discard.Refill(source);
                }
                cards.Add(source.Draw());
            }
        }

        /// <summary>
        /// Places the specified card back into its respective CardSource
        /// </summary>
        /// <param name="card">The card to replace</param>
        /// <returns>True if the discard was successful (the card was in the Hand and was removed), false otherwise.</returns>
        public Card Discard(Card card) {
            if (cards.Contains(card)) {
                return Discard((uint)cards.IndexOf(card));
            }
            return null;
        }

        /// <summary>
        /// Places the card at the specified index back into its respective deck
        /// </summary>
        /// <param name="index">The index of the card to discard</param>
        public Card Discard(uint index = 0) {
            Card r = cards[(int)index];
            if (this.discard.add(r)) {
                this.cards.Remove(r);
                return r;
            } else {
                throw new InvalidOperationException("The CardCollection you are trying to replace this Card in already has its full capacity of this Card.");
            }
        }

        /// <summary>
        /// Iteratively discards all cards in the Hand
        /// </summary>
        public List<Card> DiscardAll() {
            List<Card> r = new List<Card>(cards);
            while (cards.Count > 0) {
                Discard();
            }
            return r;
        }

        /// <summary>
        /// Returns a String representation of this Hand
        /// </summary>
        public override string ToString() {
            List<string> s = new List<string>();
            foreach (Card c in cards) {
                s.Add(c.ToString());
            }

            return String.Format("{0} [{1}]", cards.Count, String.Join<string>(", ", s.ToArray()));
        }

        /// <summary>
        /// The number of cards in the Hand
        /// </summary>
        public uint Count {
            get {
                return (uint)cards.Count;
            }
        }

        /// <summary>
        /// The first card in the Hand (the first drawn)
        /// </summary>
        public Card Top {
            get { return cards[0]; }
        }
        /// <summary>
        /// The last card in the Hand (the last drawn)
        /// </summary>
        public Card Bottom {
            get { return cards[cards.Count - 1]; }
        }

        /// <summary>
        /// Gets the Card at the specified index
        /// </summary>
        /// <param name="index">The index of the desired card (0-indexed from Top)</param>
        public Card this[int index] {
            get { return cards[index]; }
        }
    }
}
