﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
    /// <summary>
    /// An ordered collection of Cards
    /// </summary>
    class Hand
    {
        /// <summary>
        /// The shoe this Hand is associated with
        /// </summary>
        private Shoe myParent;
        /// <summary>
        /// The collection of Cards that make up this Hand
        /// </summary>
        protected List<Card> cards;

        /// <summary>
        /// The shoe this Hand is associated with
        /// </summary>
        public Shoe ParentShoe
        {
            get
            {
                return myParent;
            }
        }

        /// <summary>
        /// Creates a new Hand from a single Deck of a specifies size
        /// </summary>
        /// <remarks>As the Hand is associated with a Shoe (not a Deck), a new Shoe is created containing only this Deck.</remarks>
        /// <param name="size">The size of the new Hand</param>
        public Hand(Deck deck, int size) : this(new Shoe(deck), size) {}
        /// <summary>
        /// Creates a new Hand from a Shoe of a specifies size
        /// </summary>
        /// <param name="size">The size of the new Hand</param>
        public Hand(Shoe shoe, int size)
        {
            this.myParent = shoe;
            this.cards = new List<Card>();
            this.Draw(size);
        }

        /// <summary>
        /// Draws the specified number of cards from the parent shoe
        /// </summary>
        /// <param name="size">The number of cards to draw, defaulting to one</param>
        public void Draw(int size = 1)
        {
            for (int i = 0; i < size; i++)
            {
                cards.Add(ParentShoe.Draw());
            }
        }

        /// <summary>
        /// Places the specified card back into its respective deck
        /// </summary>
        /// <param name="card">The card to replace</param>
        /// <returns>True if the discard was successful (the card was in the Hand and was removed), false otherwise.</returns>
        public bool Discard(Card card)
        {
            if (cards.Contains(card)) {
                card.Replace();
                this.cards.Remove(card);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Places the card at the specified index back into its respective deck
        /// </summary>
        /// <param name="index">The index of the card to discard</param>
        public Card Discard(int index)
        {
            Card r = cards[index];
            this.cards.Remove(r);
            r.Replace();
            return r;
        }

        /// <summary>
        /// Discards all cards in the Hand
        /// </summary>
        public List<Card> DiscardAll()
        {
            List<Card> r = new List<Card>(cards);
            while (cards.Count > 0) {
                Discard(0);
            }
            return r;
        }

        /// <summary>
        /// Returns a String representation of this Hand
        /// </summary>
         public override string ToString()
        {
            List<string> s = new List<string>();
            foreach (Card c in cards) {
                s.Add(c.ToString());
            }

            return String.Format("{0} [{1}]", cards.Count, String.Join<string>(", ", s.ToArray()));
        }

         /// <summary>
         /// The number of cards in the Hand
         /// </summary>
        public int Count
        {
            get
            {
                return cards.Count;
            }
        }

        /// <summary>
        /// Gets the Card at the specified index
        /// </summary>
        /// <param name="index">The index of the desired card (0-indexed from Top)</param>
        public Card At(int index)
        {
            return cards[index];
        }

        /// <summary>
        /// The first card in the Hand (the first drawn)
        /// </summary>
        public Card Top
        {
            get { return cards[0]; }
        }
        /// <summary>
        /// The last card in the Hand (the last drawn)
        /// </summary>
        public Card Bottom
        {
            get { return cards[cards.Count - 1]; }
        }
    }
}
