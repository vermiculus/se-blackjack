using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
    class Deck
    {
        List<Card> cards;
        public Deck()
        {
            cards = new List<Card>();
            foreach (Suit s in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank r in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card(r, s));
                }
            }
        }

        public bool IsEmpty {
            get
            {
                return Count == 0;
            }
        }

        public int Count
        {
            get
            {
                return cards.Count;
            }
        }

        public Card Draw()
        {
            if (IsEmpty)
                return null;
            Card r = cards[(new Random()).Next(cards.Count)];
            cards.Remove(r);
            return r;
        }

        public string ToString()
        {
            return cards.Count + " " + cards.ToString();
        }

        public void ReplaceCard(Card c) {
            if (!cards.Contains(c)) {
                cards.Add(c);
            }
        }
    }
}
