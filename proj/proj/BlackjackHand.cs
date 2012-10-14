using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    /// <summary>
    /// Extends a normal Hand and adds functionality specific to the game Blackjack
    /// </summary>
    abstract class BlackjackHand : Hand {
        /*private bool isHard;

        public bool IsHard {
            get { return isHard; }
        }*/

        /// <summary>
        /// Creates a new Blackjack Hand from the specified Deck
        /// </summary>
        /// <remarks>As its superclass Hand, the given Deck is put in a Shoe for operations.</remarks>
        public BlackjackHand(Deck d, uint size = 2) : this(new Shoe(d), size) { }

        /// <summary>
        /// Creates a new Blackjack Hand from the specified Shoe
        /// </summary>
        /// <param name="s">The parent Shoe this Hand si to be associated with</param>
        /// <param name="size">The size of this hand, defaulting to two. If this hand is split, set this to zero.</param>
        // TODO: Find a better way to handle splits when it comes to the constructor. Splitting functionality should be strictly limited to the PlayerHand.
        public BlackjackHand(Shoe s, uint size = 2) : base(s, size) { }

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
        public bool IsBlackjack {
            get {
                return Count == 2 && (   (this[0].Rank == Rank.Ace && this[0].Suit == Suit.Spades) && (this[1].Rank == Rank.Jack && (this[1].Suit == Suit.Spades || this[1].Suit == Suit.Clubs))
                                      || (this[1].Rank == Rank.Ace && this[1].Suit == Suit.Spades) && (this[0].Rank == Rank.Jack && (this[0].Suit == Suit.Spades || this[0].Suit == Suit.Clubs))
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
        private static uint cardValue(Card c) {
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
        /// Does appropriate logic for a turn
        /// </summary>
        /// <param name="action">Action to do, defaulting to None</param>
        public abstract bool doTurn(BlackjackAction action = BlackjackAction.None);
        /// <summary>
        /// Places all cards this object is responsible for back in their respective decks
        /// </summary>
        public abstract void PutCardsBack(); // TODO: make this method name not suck
    }
}
