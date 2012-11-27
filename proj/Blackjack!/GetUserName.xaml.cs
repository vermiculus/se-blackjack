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
    /// Interaction logic for GetUserName.xaml
    /// </summary>
    public partial class GetUserName : Window {
        GameServant game;
        public GetUserName() {
            InitializeComponent();
            txtName.Focus();
        }

        public GetUserName(GameServant g)
            : this() {
            game = g;
        }

        public void doClick() {
            if (txtName.Text.Length > 0) {
                game.PlayerName = txtName.Text;
                this.Close();
            } else {
                MessageBox.Show("Please enter a name.");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            doClick();
        }

        private void accept(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                doClick();
            }
        }

        private void closing(object sender, System.ComponentModel.CancelEventArgs e) {

        }
    }
}
