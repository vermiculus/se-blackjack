using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack
{
    class OutOfCardsException : Exception
    {
        string msg;
        public string Message
        {
            get { return this.msg; }
        }
        public OutOfCardsException(string message = "Out of cards!")
        {
            this.msg = message;
        }
    }
    class CardCollection {
        /// <summary>
        /// The collection Cards that make up the Shoe
        /// </summary>
        protected List<Card> cards;

        private uint original_deck_size;

        public uint Count {
            get { return (uint)cards.Count; }
        }
        public bool IsEmpty {
            get { return Count == 0; }
        }

        /// <summary>
        /// Returns a random card from this Source, removing it internally
        /// </summary>
        public virtual Card Draw() {
            if (IsEmpty)
                throw new OutOfCardsException();
            Card r = cards[(new Random()).Next(cards.Count)];
            cards.Remove(r);
            return r;
        }

        /// <summary>
        /// Creates a Shoe containing the specified number of Decks
        /// </summary>
        /// <param name="count">The number of Decks to create</param>
        public CardCollection(uint deckCount = 0, bool makeCards = true) {
            original_deck_size = deckCount;
            cards = new List<Card>();
            if (makeCards) {
                for (int i = 0; i < deckCount; i++) { // for each deck,
                    foreach (Suit s in Enum.GetValues(typeof(Suit))) { // for each possible Suit
                        foreach (Rank r in Enum.GetValues(typeof(Rank))) { // for each possible Rank
                            cards.Add(new Card(r, s));
                        }
                    }
                }
                Sort();
            }
        }

        /// <summary>
        /// The total number of Decks that make up the Shoe
        /// </summary>
        public uint DeckCount {
            get { return original_deck_size; }
        }

        internal bool add(Card c) {
            if (count(c) < original_deck_size) {
                cards.Add(c);
                return true;
            }
            return false;
        }

        private uint count(Card c) {
            uint sum = 0;
            foreach (Card card in cards) {
                if (c.Equals(card)) {
                    sum++;
                }
            }
            return sum;
        }

        public void Sort() {
            cards.Sort();
        }

        /// <summary>
        /// Refill some other CardCollection with the cards from this CardCollection
        /// </summary>
        /// <param name="destination"></param>
        internal void Refill(CardCollection destination) {
            destination.cards.InsertRange(0, cards);
            cards.Clear();
        }
    }

    /// <summary>
    /// A class created for the sole purpose of testing
    /// subclassed because I'm lazy and Molly is smarter than me
    /// </summary>
    class PredeterministicCardCollection : CardCollection
    {
        Queue<Card> drawPile;
        public PredeterministicCardCollection(Queue<Card> drawpile)
        {
            drawPile = drawpile;
        }
        public PredeterministicCardCollection(string path) : this(PredeterministicCardCollection.read(path)) { }
        public override Card Draw()
        {
            if (drawPile.Count > 0)
            {
                return drawPile.Dequeue();
            }
            else
            {
                throw new OutOfCardsException();
            }
        }
        
        /// <summary>
        /// Reads a file whose contents contain the card-draw-order separated by some delimiter, defaulting to a newline.
        /// Cards must be of the format:
        /// <code>
        /// RS => Rank,Suit
        /// </code>
        /// where ranks are (2,3,4,5,6,7,8,9,0,J,Q,K) and suits are (S,H,D,C)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Queue<Card> read(string path, char delim = '\n')
        {
            Queue<Card> ret = new Queue<Card>();
            Func<string, InvalidCastException> invalid = 
                (s) => new InvalidCastException(String.Format("Cannot convert {} to a Card - it is malformed.", s));

            System.IO.StreamReader f = new System.IO.StreamReader(path);
            string buf = f.ReadToEnd();
            f.Close();
            buf = buf.Replace(Environment.NewLine, "\n");

            string[] tokens = buf.Split(delim);
            foreach (string cardid in tokens)
            {
                if (cardid.Length != 2)
                {
                    throw invalid(cardid);
                }
                try
                {
                    Rank r = (Rank)(cardid[0]);
                    Suit s = (Suit)(cardid[1]);
                    ret.Enqueue(new Card(r, s));
                }
                catch (Exception)
                {
                    throw invalid(cardid);
                }
            }

            return ret;
        }
    }
}
