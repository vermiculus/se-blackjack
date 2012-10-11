using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proj
{
    internal class Hand : PlayerHand
    {
        private List<Card> myCards;
        /// <summary>
        /// Stores a flag denoting whether the Hand has stood yet.
        /// </summary>
        private bool isStanding;
        /// <summary>
        /// A list of Cards that this Hand currently controls. When the Hand is destroyed, the Cards are returned to the Shoe.
        /// </summary>
        internal List<Card> Cards
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Calls the owning Shoe's Draw() method with itself as an argument.
        /// </summary>
        public void Hit()
        {
            throw new System.NotImplementedException();
        }

        public void Stand()
        {
            throw new System.NotImplementedException();
        }
    }
}
