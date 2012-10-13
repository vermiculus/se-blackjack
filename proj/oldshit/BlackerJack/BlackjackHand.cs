using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackerJack {
    class BlackjackHand : Hand {
        private bool isHard;

        public double Bet {
            get { return myBet; }
            set { myBet = value; }
        }

        public bool IsHard {
            get { return isHard; }
        }

        public BlackjackHand(Deck d) : base(d, 2) { }
        public BlackjackHand(Shoe s) : base(s, 2) { }
        public BlackjackHand(Shoe s, bool shouldDraw) : base(s, shouldDraw ? 0 : 2) { }

        /// <summary>
        /// Returns the highest possible sum for the hand without busting.
        /// If a bust is inevitable, the sum is returned as-is.
        /// </summary>
        public int Sum {
            get {
                int s = sumCards(cards);
                int n = NumberOfAces;
                int h = 0;
                s += 10 * n;

                while (s > 21) {
                    s -= 10;
                    h++;
                }

                isHard = (h == n);

                return s;
            }
        }

        public bool IsBust {
            get {
                return this.Sum > 21;
            }
        }

        public bool IsPerfect {
            get {
                return this.Sum == 21;
            }
        }

        public int NumberOfAces {
            get {
                int r = 0;
                foreach (Card c in cards) {
                    if (c.Rank == Rank.Ace) {
                        r++;
                    }
                }
                return r;
            }
        }

        private static int sumCards(List<Card> cards) {
            int s = 0;
            foreach (Card c in cards) {
                s += cardValue(c);
            }
            return s;
        }

        /// <summary>
        /// Gets the point value of the passed Card.
        /// </summary>
        /// <param name="c">A Card to determine the point value of</param>
        /// <returns>the point value of the passed Card</returns>
        private static int cardValue(Card c) {
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

        public bool CanSplit {
            get {
                return this.Count == 2 && this.cards[0].Equals(this.cards[1]);
            }
        }
    }
}
