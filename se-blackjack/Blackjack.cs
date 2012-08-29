using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace se_blackjack
{
    public partial class Blackjack : Form
    {
        BlackjackGame game;
        
        public Blackjack()
        {
            InitializeComponent(); // This just draws all of the controls on the screen and sets their properties
            game = new BlackjackGame();
            updateCashDisplay();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            game.Cash -= (int)numericUpDown1.Value;
            numericUpDown1.Value = numericUpDown1.Minimum;
            updateCashDisplay();
        }

        private void updateCashDisplay()
        {
            toolStripStatusLabel2.Text = "$" + game.Cash;
            numericUpDown1.Maximum = game.Cash;
        }
    }

    public class BlackjackGame
    {
        /// <summary>
        /// All possible Suites for a playing card
        /// </summary>
        internal enum Suite
        {
            Hearts,
            Spades,
            Diamonds,
            Clubs
        }

        /// <summary>
        /// All possible Ranks for a playing card
        /// </summary>
        internal enum Rank
        {
            Ace,
            King,
            Queen,
            Jack,
            Ten,
            Nine,
            Eight,
            Seven,
            Six,
            Five,
            Four,
            Three,
            Two
        }

        /// <summary>
        /// Represents a standard playing card
        /// </summary>
        internal class Card
        {
            /// <summary>
            /// The suite of this Card
            /// </summary>
            public readonly Suite Suite;

            /// <summary>
            /// The rank of this Card
            /// </summary>
            public readonly Rank Rank;

            /// <summary>
            /// Creates an immutable Card
            /// </summary>
            /// <param name="s">the Card's suite - this parameter is final</param>
            /// <param name="r">the Card's rank - this parameter is final</param>
            public Card(Suite s, Rank r)
            {
                this.Suite = s;
                this.Rank = r;
            }

            /// <summary>
            /// Creates a textual representation of the card.
            /// </summary>
            /// <returns>A textual representation of the card</returns>
            /// <example>
            /// Card(Suite.HEARTS, Rank.TEN).ToString() => "Ten of Hearts"
            /// </example>
            public override string ToString()
            {
                return this.Rank.ToString() + " of " + this.Suite.ToString();
            }
        }

        internal class Deck
        {
            private List<Card> cards;

            public Deck()
            {
                cards = new List<Card>();

                foreach (Suite s in Enum.GetValues(typeof(Suite)).Cast<Suite>()) // create an enumerable collection of all possible Suites and iterate through them
                {
                    foreach (Rank r in Enum.GetValues(typeof(Rank)).Cast<Rank>()) // and do the same for Ranks
                    {
                        this.cards.Add(new Card(s, r)); // Add every possible combination of Rank and Suite to the Deck
                    }
                }

                foreach (var card in cards)
                {
                    Console.WriteLine(card.ToString());
                }
            }

            public Card draw()
            {
                var r = new Random();
                int i = r.Next(cards.Count);
                Card ret = cards[i];
                cards.RemoveAt(i);
                return ret;
            }
        }

        /// <summary>
        /// Available cash to the Player
        /// </summary>
        public int Cash { get; set; } // Note that Java's convention of 'get' and 'set' methods are obselete in C#
        private Deck deck;
        
        public BlackjackGame(int cash)
        {
            this.deck = new Deck();
            this.Cash = cash;
        }

        public BlackjackGame() : this(500) { }
    }
}
