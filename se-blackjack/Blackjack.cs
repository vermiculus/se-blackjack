using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace se_blackjack
{
    public partial class Blackjack : Form
    {
        BlackjackGame game;
        public Blackjack()
        {
            InitializeComponent(); // This just draws all of the controls on the screen and sets their properties
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game = new BlackjackGame();
            updateCashDisplay();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            game.Cash -= (int)numericUpDown1.Value;
            numericUpDown1.Value = numericUpDown1.Minimum;
            updateCashDisplay();
        }
        private void updateCashDisplay()
        {
            toolStripStatusLabel2.Text = "$" + game.Cash;
            numericUpDown1.Maximum = game.Cash;
        }
    }

    public class BlackjackGame
    {
        public int Cash { get; set; } // Note that Java's convention of 'get' and 'set' methods are obselete in C#
        public BlackjackGame()
        {
            this.Cash = 500;
        }
    }
}
