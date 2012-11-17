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
    /// Interaction logic for UI_Sketch.xaml
    /// </summary>
    public partial class UI_Sketch : Window
    {
        GameServant game;
        public UI_Sketch()
        {
            InitializeComponent();
            game = new GameServant();
            //game = FindResource("game") as GameServant;
            (new GetUserName(ref game)).ShowDialog();
            game.init();
            (new GetBet(ref game)).ShowDialog();
            this.DataContext = game;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            switch (game.ActiveHand)
            {
                case GameServant.ActiveHandPotentials.Normal:
                    game.ActiveHand = GameServant.ActiveHandPotentials.Split;
                    break;
                case GameServant.ActiveHandPotentials.Split:
                    game.ActiveHand = GameServant.ActiveHandPotentials.Normal;
                    break;
                default:
                    break;
            }
        }

        private void btnNormalHit_Click(object sender, RoutedEventArgs e)
        {
            game.Hit();
        }

        private void draw()
        {

        }

        private void btnNormalStand_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
