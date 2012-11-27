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
    /// Interaction logic for GetBet.xaml
    /// </summary>
    public partial class GetBet : Window {
        public uint Bet;
        public GameServant game;

        public GetBet(GameServant g) {
            InitializeComponent();
            this.Title += String.Format(" ({0} available)", g.PlayerFunds);
            game = g;
            txtBet.Focus();
        }

        private void end() {
            if (txtBet.Text == "" || int.Parse(txtBet.Text) < GameServant.MIN_BET || int.Parse(txtBet.Text) > game.PlayerFunds) {
                MessageBox.Show(String.Format("The bet must be between {0} and {1}", GameServant.MIN_BET, game.PlayerFunds));
            } else {
                Bet = uint.Parse(txtBet.Text);
                this.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void keydown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                end();
            }
            if (!(
                e.Key == Key.D0 || e.Key == Key.D1 || e.Key == Key.D2 || e.Key == Key.D3 || e.Key == Key.D4 ||
                e.Key == Key.D5 || e.Key == Key.D6 || e.Key == Key.D7 || e.Key == Key.D8 || e.Key == Key.D9 ||
                e.Key == Key.NumPad0 || e.Key == Key.NumPad1 || e.Key == Key.NumPad2 || e.Key == Key.NumPad3 || e.Key == Key.NumPad4 ||
                e.Key == Key.NumPad5 || e.Key == Key.NumPad6 || e.Key == Key.NumPad7 || e.Key == Key.NumPad8 || e.Key == Key.NumPad9)) {
                e.Handled = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            end();
        }

        private void onclose(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
        }
    }
}
