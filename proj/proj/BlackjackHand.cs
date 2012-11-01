using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack {
    /// <summary>
    /// Extends a normal Hand and adds functionality specific to the game Blackjack
    /// </summary>
    abstract class BlackjackHand : Hand {

        /// <summary>
        /// Creates a new Blackjack Hand from the specified Shoe
        /// </summary>
        /// <param name="s">The parent Shoe this Hand si to be associated with</param>
        /// <param name="size">The size of this hand, defaulting to two. If this hand is split, set this to zero.</param>
        // TODO: Find a better way to handle splits when it comes to the constructor. Splitting functionality should be strictly limited to the PlayerHand.
        public BlackjackHand(CardCollection source, CardCollection discard) : base(source, discard) { }

        /// <summary>
        /// The highest possible sum for the hand without busting.
        /// If a bust is inevitable, the sum is returned as-is.
        /// </summary>
        public uint Sum {
            get {
                uint s = sumCards(cards);
                uint n = NumberOfAces;
                while (s > 21 && n > 0) {
                    s -= 10;
                    n--;
                }
                return s;
            }
        }

        /// <summary>
        /// True if Sum &gt; 21
        /// </summary>
        public bool IsBust {
            get {
                return this.Sum > 21;
            }
        }

        /// <summary>
        /// True if the hand is a combination of the Ace of Spades and some Jack
        /// </summary>
        public bool IsBlackjack { // SRS 1.3 2.3.2
            get {
                return Count == 2 && (   this[0].Rank == Rank.Ace && this[1].IsFaceCard
                                      || this[1].Rank == Rank.Ace && this[0].IsFaceCard
                                     );
            }
        }

        /// <summary>
        /// The number of aces in the Hand
        /// </summary>
        public uint NumberOfAces {
            get {
                uint r = 0;
                foreach (Card c in cards) {
                    if (c.Rank == Rank.Ace) {
                        r++;
                    }
                }
                return r;
            }
        }

        /// <summary>
        /// Returns the sum of all the cards in the given collection, ace high
        /// </summary>
        /// <param name="cards">The collection of cards to sum</param>
        protected static uint sumCards(List<Card> cards) {
            uint s = 0;
            foreach (Card c in cards) {
                s += cardValue(c);
            }
            return s;
        }

        /// <summary>
        /// Gets the point value of the passed Card, ace high
        /// </summary>
        /// <param name="c">A Card to determine the point value of</param>
        /// <returns>the point value of the passed Card</returns>
        public static uint cardValue(Card c) {
            switch (c.Rank) {
                case Rank.Ace:
                    return 11;
                case Rank.Two:
                    return 2;
                case Rank.Three:
                    return 3;
                case Rank.Four:
                    return 4;
                case Rank.Five:
                    return 5;
                case Rank.Six:
                    return 6;
                case Rank.Seven:
                    return 7;
                case Rank.Eight:
                    return 8;
                case Rank.Nine:
                    return 9;
                case Rank.Ten:
                case Rank.Jack:
                case Rank.Queen:
                case Rank.King:
                    return 10;
                default:
                    throw new ArgumentException("That Rank is not recognized! Are you doing any funny casting business? Bad programmer!");
            }
        }

        /// <summary>
        /// Places all cards this object is responsible for back in their respective decks
        /// </summary>
        public abstract void PutCardsBack(); // TODO: make this method name not suck

        internal void giveCards(Card a, Card b)
        {
            Draw();
            while (!this[0].Equals(a))
            {
                Discard(0);
                Draw();
            }
            Draw();
            while (!this[1].Equals(b))
            {
                Discard(1);
                Draw();
            }
        }
    }
}
