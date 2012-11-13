using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for CardStack.xaml
    /// </summary>
    public partial class CardStack : UserControl
    {
        private List<Card> cards;
        private static Card[]
            RoyalFlush = {
                             new Card(Rank.King, Suit.Hearts) };

        public static readonly DependencyProperty CardsProperty =
            DependencyProperty.Register(
                "Cards",
                typeof(List<Card>),
                typeof(CardStack),
                new PropertyMetadata(new List<Card>())
            );

        public List<Card> Cards
        {
            get { return (List<Card>)GetValue(CardsProperty); }
            set
            {
                SetValue(CardsProperty, value);
                draw();
            }
        }

        public CardStack()
        {
            InitializeComponent();
        }

        private void draw()
        {
            int lm = 0;
            foreach (Card card in cards)
            {
                Label cardDisplay = new Label();
                cardDisplay.Content = getCardImage(card);
                cardDisplay.Margin = new Thickness(lm, 0, 0, 0);
                this.AddChild(cardDisplay);
                lm += 14;
            }
        }

        private object getCardImage(Card c)
        {
            switch (c.Suit)
            {
                case Suit.Hearts:
                    switch (c.Rank)
                    {
                        case Rank.Ace:
                            return Properties.Resources.h1;
                        case Rank.Two:
                            return Properties.Resources.h2;
                        case Rank.Three:
                            return Properties.Resources.h3;
                        case Rank.Four:
                            return Properties.Resources.h4;
                        case Rank.Five:
                            return Properties.Resources.h5;
                        case Rank.Six:
                            return Properties.Resources.h6;
                        case Rank.Seven:
                            return Properties.Resources.h7;
                        case Rank.Eight:
                            return Properties.Resources.h8;
                        case Rank.Nine:
                            return Properties.Resources.h9;
                        case Rank.Ten:
                            return Properties.Resources.h10;
                        case Rank.Jack:
                            return Properties.Resources.hj;
                        case Rank.Queen:
                            return Properties.Resources.hq;
                        case Rank.King:
                            return Properties.Resources.hk;
                        default:
                            break;
                    }
                    break;
                case Suit.Spades:
                    switch (c.Rank)
                    {
                        case Rank.Ace:
                            return Properties.Resources.s1;
                        case Rank.Two:
                            return Properties.Resources.s2;
                        case Rank.Three:
                            return Properties.Resources.s3;
                        case Rank.Four:
                            return Properties.Resources.s4;
                        case Rank.Five:
                            return Properties.Resources.s5;
                        case Rank.Six:
                            return Properties.Resources.s6;
                        case Rank.Seven:
                            return Properties.Resources.s7;
                        case Rank.Eight:
                            return Properties.Resources.s8;
                        case Rank.Nine:
                            return Properties.Resources.s9;
                        case Rank.Ten:
                            return Properties.Resources.s10;
                        case Rank.Jack:
                            return Properties.Resources.sj;
                        case Rank.Queen:
                            return Properties.Resources.sq;
                        case Rank.King:
                            return Properties.Resources.sk;
                        default:
                            break;
                    }
                    break;
                case Suit.Diamonds:
                    switch (c.Rank)
                    {
                        case Rank.Ace:
                            return Properties.Resources.d1;
                        case Rank.Two:
                            return Properties.Resources.d2;
                        case Rank.Three:
                            return Properties.Resources.d3;
                        case Rank.Four:
                            return Properties.Resources.d4;
                        case Rank.Five:
                            return Properties.Resources.d5;
                        case Rank.Six:
                            return Properties.Resources.d6;
                        case Rank.Seven:
                            return Properties.Resources.d7;
                        case Rank.Eight:
                            return Properties.Resources.d8;
                        case Rank.Nine:
                            return Properties.Resources.d9;
                        case Rank.Ten:
                            return Properties.Resources.d10;
                        case Rank.Jack:
                            return Properties.Resources.dj;
                        case Rank.Queen:
                            return Properties.Resources.dq;
                        case Rank.King:
                            return Properties.Resources.dk;
                        default:
                            break;
                    }
                    break;
                case Suit.Clubs:
                    switch (c.Rank)
                    {
                        case Rank.Ace:
                            return Properties.Resources.c1;
                        case Rank.Two:
                            return Properties.Resources.c2;
                        case Rank.Three:
                            return Properties.Resources.c3;
                        case Rank.Four:
                            return Properties.Resources.c4;
                        case Rank.Five:
                            return Properties.Resources.c5;
                        case Rank.Six:
                            return Properties.Resources.c6;
                        case Rank.Seven:
                            return Properties.Resources.c7;
                        case Rank.Eight:
                            return Properties.Resources.c8;
                        case Rank.Nine:
                            return Properties.Resources.c9;
                        case Rank.Ten:
                            return Properties.Resources.c10;
                        case Rank.Jack:
                            return Properties.Resources.cj;
                        case Rank.Queen:
                            return Properties.Resources.cq;
                        case Rank.King:
                            return Properties.Resources.ck;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            throw new InvalidOperationException("Invalid Card given");
        }
    }
}
