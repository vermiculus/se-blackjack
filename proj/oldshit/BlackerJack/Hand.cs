using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackerJack
{
    class Hand
    {
        private Shoe myParent;
        protected List<Card> cards;

        public Shoe ParentShoe
        {
            get
            {
                return myParent;
            }
        }

        public Hand(Deck deck, int size) : this(new Shoe(deck), size) {}
        public Hand(Shoe shoe, int size)
        {
            this.myParent = shoe;
            this.cards = new List<Card>();
            this.Draw(size);
        }

        public void Draw(int size)
        {
            for (int i = 0; i < size; i++)
            {
                cards.Add(ParentShoe.Draw());
            }
        }
        public void Draw()
        {
            Draw(0);
        }

        public bool Discard(Card card)
        {
            return this.cards.Remove(card);
        }

        public Card Discard(int index)
        {
            Card r = cards[index];
            this.cards.Remove(r);
            return r;
        }

        public List<Card> DiscardAll()
        {
            List<Card> r = new List<Card>(cards);
            cards.Clear();
            return r;
        }

        public string ToString()
        {
            return cards.Count + " " + cards.ToString();
        }

        public int Count
        {
            get
            {
                return cards.Count;
            }
        }

        public Card At(int index)
        {
            return cards[index];
        }

        public Card Top
        {
            get { return cards[0]; }
        }
        public Card Bottom
        {
            get { return cards[cards.Count - 1]; }
        }
    }
}
