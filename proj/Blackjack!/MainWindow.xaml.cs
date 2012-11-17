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
            (new UI_Sketch()).Show();
            //this.Close();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void MenuItem_Statistics_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show(String.Format("Wins-Losses: {0}-{1}\nBiggest Win: {2}\nBiggest Loss: {3}",
                g.NumWins, g.NumLosses, g.LargestWin, g.LargestLoss), "Statistics", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void button1_Click(object sender, RoutedEventArgs e) {
            cardstack.Add((Rank)comboBox1.SelectedItem, (Suit)comboBox2.SelectedItem);
        }
    }
}
