using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proj
{
    public class PlayerHand
    {
        /// <summary>
        /// Denotes whether the hand has already has already been split. That is, when a PlayerHand is split, a new PlayerHand is created and the original cards are split among the two. At that point, both feilds hasSplit are set to true.
        /// </summary>
        private bool hasSplit;

        /// <summary>
        /// For use with splitting.
        /// </summary>
        /// <param name="splitCard">The Card that this <em>split</em> hand should contain. This Card should be removed from the calling PlayerHand prior to this moment.</param>
        protected PlayerHand(Card splitCard)
        {
            throw new System.NotImplementedException();
        }
    
        public bool Splittable
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void Split()
        {
            throw new System.NotImplementedException();
        }
    }
}
