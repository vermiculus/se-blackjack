using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackerJack
{
    public class Card : IComparable<Card>
    {
        Suit mySuite;
        Rank myRank;

        public Rank Rank
        {
            get
            {
                return myRank;
            }
            set
            {
                myRank = value;
            }
        }
        public Suit Suite
        {
            get
            {
                return mySuite;
            }
            set
            {
                mySuite = value;
            }
        }

        public Card(Rank rank, Suit suite)
        {
            this.Rank = rank;
            this.Suite = suite;
        }

        /// <summary>
        /// Returns a String representation of the Card
        /// </summary>
        public String ToString()
        {
            throw new System.NotImplementedException();
        }

        public int CompareTo(Card c)
        {
            if (c.Suite > this.Suite)
                return 1;
            else if (c.Suite < this.Suite)
                return -1;
            else if (c.Rank > this.Rank)
                return 1;
            else if (c.Rank < this.Rank)
                return -1;
            else return 0;
        }

        public bool Equals(Object obj)
        {
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            else
            {
                return this.CompareTo((Card)obj) == 0;
            }
        }
    }
}
