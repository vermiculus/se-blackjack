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

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for GetUserName.xaml
    /// </summary>
    public partial class GetUserName : Window
    {
        GameServant codebehind;
        public GetUserName()
        {
            InitializeComponent();
            txtName.Focus();
        }

        public GetUserName(ref GameServant game) : this()
        {
            game = this.FindResource("game") as GameServant;
            codebehind = game;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Length > 0)
            {
                codebehind.PlayerName = txtName.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a name.");
            }
        }

        private void accept(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click_1(null, null);
            }
        }
    }
}
