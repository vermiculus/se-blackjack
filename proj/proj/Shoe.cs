using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proj
{
    public class Shoe
    {
        internal List<Hand> ActiveHands
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        internal List<Deck> Decks
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        /// <summary>
        /// Draws a random card from the deck and places it in the specified Hand.
        /// </summary>
        /// <returns>
        /// A reference to the Card placed in the specified Hand.
        /// Returns an error if the specified Hand is not found.
        /// </returns>
        public Card Draw(int handID)
        {
            throw new System.NotImplementedException();
        }
    }
}
