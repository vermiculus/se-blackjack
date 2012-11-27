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
        public void init(bool fullrestart=true) {
            if (fullrestart) {
                game = new GameServant();
                //game = FindResource("game") as GameServant;
                (new GetUserName(ref game)).ShowDialog();
                game.init();
                this.DataContext = game;
            }
            game.GetBetAndDeal();
            paint();
        }

        private void paint() {
            csPlayerNormal.Cards = game.PlayerNormalCards;
            if (game.PlayerHand.HasSplit)
                csPlayerSplit.Cards = game.PlayerSplitCards;
            if (gameover)
            {
                csDealerNormal.Cards = game.DealerHand.Cards;
            } else {
                var hole = new List<Card>(game.DealerHand.Cards);
                hole[0] = null;
                csDealerNormal.Cards = hole;
            }
            try {
                this.Visibility = System.Windows.Visibility.Visible;
            } catch (InvalidOperationException e) {
                // WHAT.  WHAT.  YOU THINK THIS IS A GAME?

                // (since this method can be called when the window is
                // closing, bad things can happen)
            }

            if (game.PlayerHand.IsBlackjack) {
                playAgain("You won!");
            }
        }

        private void btnNormalHit_Click(object sender, RoutedEventArgs e) {
            game.Hit();
            paint();
            switch (game.ActiveHand) {
                case GameServant.ActiveHandPotentials.Normal:
                    if (game.PlayerHand.IsBust) {
                        if (game.PlayerHand.HasSplit) {
                            MessageBox.Show("Busted!");
                            game.ActiveHand = GameServant.ActiveHandPotentials.Split;
                        } else {
                            playAgain("Busted! Lost the round!");
                        }
                        //gameover = true;
                    }
                    break;
                case GameServant.ActiveHandPotentials.Split:
                    if (game.PlayerHand.SplitHand.IsBust) {
                        playAgain("Busted! Lost the round!");
                    }
                    if (game.DealerHand.IsBust) {
                        playAgain("You won!");
                    }
                    break;
                case GameServant.ActiveHandPotentials.None:
                    break;
                default:
                    break;
            }
            paint();
        }

        private void playAgain(string message) {
            if (game.PlayerFunds < 20) {
                MessageBox.Show("You don't have enough cash to keep playing!", message);
                game.ActiveHand = GameServant.ActiveHandPotentials.None;
            } else if (MessageBox.Show("Play again?", message, MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                game.NewRound();
                gameover = false;
                paint();
            } else {
                this.Close();
            }
        }

        bool gameover = false;
        private void btnNormalStand_Click(object sender, RoutedEventArgs e) {
            game.Stand();
            if (game.PlayerHand.HasSplit) {
                game.ActiveHand = GameServant.ActiveHandPotentials.Split;
                paint();
            } else {
                game.ActiveHand = GameServant.ActiveHandPotentials.None;
                gameover = true;
                paint();
                endround();
            }
        }

        private void endround() {
            if (!game.PlayerHand.HasSplit) {
                switch (game.Winner(game.PlayerHand)) {
                    case WinLoss.Dealer:
                        playAgain("Dealer won!");
                        break;
                    case WinLoss.Player:
                        playAgain("You won!");
                        break;
                    case WinLoss.Push:
                        playAgain("Push!!");
                        break;
                    case WinLoss.NoWin:
                        throw new Exception("HELP!!!");
                }
            } else {
                string norm = "Hand 1: ";
                string splt = "Hand 2: ";

                switch (game.Winner(game.PlayerHand)) {
                    case WinLoss.Dealer:
                        norm += "Lost!";
                        break;
                    case WinLoss.Player:
                        norm += "Won!";
                        break;
                    case WinLoss.NoWin:
                        norm += "Push!";
                        break;
                    default:
                        throw new Exception("HELP!!!");
                }

                switch (game.Winner(game.PlayerHand.SplitHand)) {
                    case WinLoss.Dealer:
                        splt += "Lost!";
                        break;
                    case WinLoss.Player:
                        splt += "Won!";
                        break;
                    case WinLoss.NoWin:
                        splt += "Push!";
                        break;
                    default:
                        throw new Exception("HELP!!!");
                }

                playAgain(norm + Environment.NewLine + splt);
            }
        }

        private void menu_exit(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void window_exit(object sender, System.ComponentModel.CancelEventArgs e) {
            if (MessageBox.Show("Are you sure?", "Exiting Blackjack!",
                MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) != MessageBoxResult.Yes) {
                e.Cancel = true;
                game.NewRound();
                paint();
            }
        }
        
        private void menu_restart(object sender, RoutedEventArgs e) {
            this.Visibility = System.Windows.Visibility.Hidden;
            init();
            this.Visibility = System.Windows.Visibility.Visible;
        }

        private void menu_about(object sender, RoutedEventArgs e) {

        }

        private void menu_stats(object sender, RoutedEventArgs e) {
            MessageBox.Show(String.Format("Number of Wins: {0}\nNumber of Losses: {1}\nBiggest Win: {2}\nBiggest Loss: {3}",
                game.NumWins, game.NumLosses, game.LargestWin, game.LargestLoss));
        }

        private void on_load(object sender, RoutedEventArgs e) {
        }
    }
}
