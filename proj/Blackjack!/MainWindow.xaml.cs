#define FROMDISK

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
using System.IO;

namespace Blackjack {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        const int CARD_WIDTH = 71;
        Game g;

        public MainWindow() {
            InitializeComponent();
            g = new Game();

            comboBox1.ItemsSource = Enum.GetValues(typeof(Rank));
            comboBox2.ItemsSource = Enum.GetValues(typeof(Suit));
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void MenuItem_Statistics_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show(String.Format("Wins-Losses: {0}-{1}\nBiggest Win: {2}\nBiggest Loss: {3}",
                g.NumWins, g.NumLosses, g.LargestWin, g.LargestLoss), "Statistics", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void updateCardDisplay(Image im, Rank r, Suit s) {
            Func<Rank, Suit, string, string> getFilename = (rn, su, ext) =>
            {
                string rnk, sut;
                switch (rn) {
                    case Rank.Ace:
                        rnk = "1";
                        break;
                    case Rank.Two:
                        rnk = "2";
                        break;
                    case Rank.Three:
                        rnk = "3";
                        break;
                    case Rank.Four:
                        rnk = "4";
                        break;
                    case Rank.Five:
                        rnk = "5";
                        break;
                    case Rank.Six:
                        rnk = "6";
                        break;
                    case Rank.Seven:
                        rnk = "7";
                        break;
                    case Rank.Eight:
                        rnk = "8";
                        break;
                    case Rank.Nine:
                        rnk = "9";
                        break;
                    case Rank.Ten:
                        rnk = "10";
                        break;
                    case Rank.Jack:
                        rnk = "j";
                        break;
                    case Rank.Queen:
                        rnk = "q";
                        break;
                    case Rank.King:
                        rnk = "k";
                        break;
                    default:
                        rnk = null;
                        break;
                }
                switch (su) {
                    case Suit.Hearts:
                        sut = "h";
                        break;
                    case Suit.Spades:
                        sut = "s";
                        break;
                    case Suit.Diamonds:
                        sut = "d";
                        break;
                    case Suit.Clubs:
                        sut = "c";
                        break;
                    default:
                        sut = null;
                        break;
                }
                return sut + rnk + "." + ext;
            };
#if FROMDISK
            Uri myUri = new Uri(@"C:\cards\" + getFilename(r, s, "png"), UriKind.Absolute);
            PngBitmapDecoder decoder2 = new PngBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bmp = decoder2.Frames[0];
#else
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("Blackjack." + getFilename(r, s, "png"));
            BitmapImage bmp = new BitmapImage();
            bmp.StreamSource = myStream;
#endif
            // Draw the Image
            im.Source = bmp;
            im.Stretch = Stretch.Uniform;
            im.Width = CARD_WIDTH;
        }

        private void button1_Click(object sender, RoutedEventArgs e) {
            updateCardDisplay(image1, (Rank)comboBox1.SelectedItem, (Suit)comboBox2.SelectedItem);
        }
    }
}
