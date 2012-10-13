using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proj
{
    public class Shoe
    {
        /// <summary>
        /// Contains references to all hands that draw from this Shoe
        /// </summary>
        private List<Hand> myHands;
        /// <summary>
        /// Contains references to all decks that
        /// </summary>
        private List<Deck> myDecks;
    
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
