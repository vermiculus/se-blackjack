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
        private System.Timers.Timer t;

        public GetBet(GameServant g, UI_Sketch ui) {
            InitializeComponent();
            this.Title += String.Format(" ({0} available)", g.PlayerFunds.Substring(7));
            t = new System.Timers.Timer(100);
            t.Elapsed += t_Elapsed;
            game = g;
            if (ui != null) {
                ui.Visibility = System.Windows.Visibility.Visible;
            }
            if (Application.Current != null) {
                Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate {
                }));
            }
            this.ShowDialog();
            txtBet.Focus();
            //t.Start();
        }

        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            txtBet.Focus();
        }

        private void end() {
            if (txtBet.Text == "" || int.Parse(txtBet.Text) < GameServant.MIN_BET || int.Parse(txtBet.Text) > game.PlayerHand.Cash) {
                MessageBox.Show(String.Format("The bet must be between ${0} and {1}", GameServant.MIN_BET, game.PlayerFunds.Substring(7)));
            } else {
                Bet = uint.Parse(txtBet.Text);
                this.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            end();
        }

        private void onclose(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
        }

        private void txtBet_TextChanged(object sender, TextChangedEventArgs e) {
            string s = "";
            int idx = txtBet.CaretIndex;
            bool did_screw_up = false;
            foreach (char c in txtBet.Text) {
                if (char.IsDigit(c))
                    s += c;
                else
                    did_screw_up = true;
            }
            txtBet.Text = s;
            txtBet.CaretIndex = did_screw_up ? (idx > 0 ? idx - 1 : 0) : idx;
        }

        private void givefocus(object sender, RoutedEventArgs e) {
            txtBet.Focus();
        }

        private void foc(object sender, KeyEventArgs e) {
            txtBet.Focus();
        }
    }
}
