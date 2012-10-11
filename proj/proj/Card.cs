using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proj
{
    public class Card
    {
        /// <summary>
        /// Denotes the Rank of the Card
        /// </summary>
        private Rank myRank;
        /// <summary>
        /// Denotes the Suite of the Card
        /// </summary>
        private Suite mySuite;

        /// <summary>
        /// Returns all Cards to the Deck.
        /// </summary>
        ~Card()
        {
            throw new System.NotImplementedException();
        }
        // override object.Equals
        public override bool Equals (object obj)
        {
            if (obj == null || GetType() != obj.GetType()) 
            {
                return false;
            }
            // TODO: write your implementation of Equals() here
            return base.Equals(obj);    
        }
    
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return base.GetHashCode();
        }
        }

    internal enum Rank
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    internal enum Suite
    {
        Hearts,
        Spades,
        Diamonds,
        Clubs,
    }
}
