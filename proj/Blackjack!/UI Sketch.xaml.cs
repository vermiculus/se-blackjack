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
using System.Windows.Shapes;

namespace Blackjack {
    /// <summary>
    /// Interaction logic for UI_Sketch.xaml
    /// </summary>
    public partial class UI_Sketch : Window {
        GameServant game;
        public UI_Sketch() {
            InitializeComponent();
            init();
        }
        public void init() {
            this.Visibility = System.Windows.Visibility.Hidden;
            game = new GameServant();
            this.DataContext = game;
            game.NotifyAll();
            newr();
            paint();
            this.Visibility = System.Windows.Visibility.Visible;
        }

        private void newr() {
            try {
                game.NewRound();
                game.NotifyAll();
                paint();
                //throw new GameServant.BlackjackException();
            } catch (GameServant.BlackjackException) {
                paint();
                endRound(true);
            }
        }

        private void paint() {
            this.Visibility = System.Windows.Visibility.Visible;
            csPlayerNormal.Cards = game.PlayerNormalCards;
            if (game.PlayerHand.HasSplit)
                csPlayerSplit.Cards = game.PlayerSplitCards;
            else {
                csPlayerSplit.Cards = new List<Card>();
            }
            if (game.DisplayHole)
            {
                csDealerNormal.Cards = game.DealerHand.Cards;
            } else {
                var hole = new List<Card>(game.DealerHand.Cards);
                hole[0] = null;
                csDealerNormal.Cards = hole;
            }
        }

        private void btnNormalHit_Click(object sender, RoutedEventArgs e) {
            try {
                game.Hit();
            } catch (GameServant.BustedException) {
                paint();
                switchHandsIfApplicable();
            }
            paint();
        }

        private void btnNormalStand_Click(object sender, RoutedEventArgs e) {
            game.Stand();
            paint();
            switchHandsIfApplicable();
        }

        private void switchHandsIfApplicable() {
            if (game.PlayerHand.HasSplit && game.ActiveHand == GameServant.ActiveHandPotentials.Normal) {
                game.ActiveHand = GameServant.ActiveHandPotentials.Split;
            } else {
                endRound();
            }
        }

        private void endRound(bool blj = false) {
            paint();
            if (blj) {
                game.EndRound();
                playAgain("Blackjack!", true);
            } else {
                WinLoss?[] results = game.EndRound();
                string h1, h2;
                bool won;
                switch (results[0]) {
                    case WinLoss.Dealer:
                        h1 = "Lost!";
                        won = false;
                        break;
                    case WinLoss.Player:
                        h1 = "Won!";
                        won = true;
                        break;
                    case WinLoss.Push:
                        h1 = "Push!!";
                        won = true;
                        break;
                    default:
                        throw new Exception("What");
                }
                if (game.PlayerHand.HasSplit) {
                    switch (results[1]) {
                        case WinLoss.Dealer:
                            h2 = "Lost!";
                            break;
                        case WinLoss.Player:
                            h2 = "Won!";
                            break;
                        case WinLoss.Push:
                            h2 = "Push!!";
                            break;
                        default:
                            throw new Exception("What");
                    }
                    playAgain(String.Format("Hand 1: {0}  Hand 2: {1}", h1, h2), won);
                } else {
                    playAgain(h1, won);
                }
            }
        }

        private void playAgain(string msg, bool playerwon = false) {
            if (game.PlayerFunds < GameServant.MIN_BET && !playerwon) {
                MessageBox.Show("You lost the game!!! You have no more money!!", "Lost the game!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            } else if (MessageBox.Show("Play Again?", msg,
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes)
                == MessageBoxResult.Yes) {
                paint();
                newr();
                paint();
            }
            paint();
        }

        #region Window and Menu Operations

        private void menu_exit(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void window_exit(object sender, System.ComponentModel.CancelEventArgs e) {
            if (MessageBox.Show("Are you sure?", "Exiting Blackjack!",
                MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No) {
                e.Cancel = true;
            } else {
                Environment.Exit(0);
            }
        }
        
        private void menu_restart(object sender, RoutedEventArgs e) {
            this.Visibility = System.Windows.Visibility.Hidden;
            init();
            this.Visibility = System.Windows.Visibility.Visible;
        }

        private void menu_about(object sender, RoutedEventArgs e) {
            MessageBox.Show("No Dice! Blackjack program.\nVersion 1.1");
        }

        private void menu_stats(object sender, RoutedEventArgs e) {
            MessageBox.Show(String.Format("Number of Wins: {0}\nNumber of Losses: {1}\nBiggest Win: {2}\nBiggest Loss: {3}",
                game.NumWins, game.NumLosses, game.LargestWin, game.LargestLoss));
        }

        #endregion

        private void btnNormalSplit_Click(object sender, RoutedEventArgs e) {
            game.Split();
            paint();
        }
    }
}
