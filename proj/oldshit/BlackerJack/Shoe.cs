using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackerJack
{
    class Shoe
    {
        private List<Deck> decks;

        public Shoe(int count)
        {
            decks = new List<Deck>();
            for (int i = 0; i < count; i++)
            {
                decks.Add(new Deck());
            }
        }

        public Shoe(Deck deck)
        {
            this.decks = new List<Deck>();
            this.decks.Add(deck);
        }

        public Card Draw()
        {
            Random rn = new Random();
            int i = rn.Next(DeckCount);
            while (decks[i].IsEmpty)
            {
                i = rn.Next(DeckCount);
            }
            return decks[i].Draw();
        }

        public bool isEmpty
        {
            get
            {
                bool r = true;
                foreach (Deck deck in decks)
                {
                    r &= deck.IsEmpty;
                }
                return r;
            }
        }

        public int DeckCount
        {
            get
            {
                return decks.Count;
            }
        }

        public int CardCount
        {
            get
            {
                int r = 0;
                foreach (Deck d in decks)
                {
                    r += d.Count;
                }
                return r;
            }
        }
    }
}
