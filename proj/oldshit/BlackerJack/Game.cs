using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackerJack {
    class Game {

        private enum WinLoss {
            NoWin,
            Dealer,
            Player
        }

        private Shoe shoe;
        BlackjackHand player;
        BlackjackHand psplit;
        BlackjackHand dealer;

        int turn;

        private bool playerSplit;
        private bool playerDoubledDown;
        private bool endTurns;

        public Game() {
            Console.WriteLine("Welcome to Blackjack!");
            shoe = new Shoe(1);
        }

        public double PlayerBet {
            get {
                return player.Bet;
            }
            set {
                player.Bet = value;
            }
        }

        public void shuffleShoe() {
            double frac = (double)shoe.CardCount / (shoe.DeckCount * 52);
            if (frac > .75)
                return;
            if (frac <= .25) {
                shoe = new Shoe(5);
            } else {
                if (((new Random()).NextDouble() > ((int)(100 - 100 * frac))))
                // TODO: figure out what I was on and get some more
                {
                    shoe = new Shoe(5);
                }
            }
        }

        private void doTurn() {
            int c;
            if (playerDoubledDown) // TODO: Is this functionality required anymore?
                c = 2;
            else
                c = displayMenu();

            switch (c) {
                case 1:
                    hit();
                    break;
                case 2:
                    stand();
                    break;
                case 3:
                    split();
                    break;
                case 4:
                    double_down();
                    break;
            }
            if (!endTurns) {
                printHands();
                turn++;
            }
        }

        private void printHands(bool displayDealer = false) {
            if (displayDealer) {
                //  TODO: not implemented
                Console.WriteLine("Dealer's Hand: {}", dealer.ToDealerString());
            } else {
                Console.WriteLine("Dealer's Hand: {}", dealer.ToString());
            }
            if (playerSplit) {
                Console.WriteLine("Your Hand (1): {}", player.ToString());
                Console.WriteLine("          (2): {}", psplit.ToString());
            } else {
                Console.WriteLine("    Your Hand: {}", player.ToString());
            }
        }

        private void Play() {
            player = new BlackjackHand(shoe);
            dealer = new BlackjackHand(shoe);

            turn = 1;
            playerSplit = false;
            printHands();

            // TODO: incorrect [from old source -- dunno what this means]
            while (checkWinLoss() == WinLoss.NoWin) {
                doTurn();
            }
        }

        private WinLoss checkWinLoss() {
            // TODO: verify accuracy?
            if (player.IsBust || dealer.IsPerfect)
                return WinLoss.Dealer;
            if (dealer.IsBust || player.IsPerfect)
                return WinLoss.Player;
            return WinLoss.NoWin;
        }

        private void hit() {
            player.Draw();
            if (playerSplit) {
                psplit.Draw();
            }
        }

        private void stand() {
            while (dealerTurn()) ;

            printHands(true);
            endTurns = true;
            end();
        }

        private void __dispDealerWin() {
            Console.WriteLine("You lost with {} points under the dealer's hand of {} points. :[", player.Sum, dealer.Sum);
        }

        private void __dispPlayerWin() {
            Console.WriteLine("You won with {} points over the dealer's hand of {} points. :D", player.Sum, dealer.Sum);
        }

        private void __dispTie() {
            Console.WriteLine("You tied the dealer with {} points! :O", player.Sum);
        }

        private void end() {
            if (endTurns) {
                switch (checkWinLoss()) {
                    case WinLoss.NoWin:
                        if (player.Sum > dealer.Sum) {
                            __dispPlayerWin();
                        } else if (player.Sum < dealer.Sum) {
                            __dispDealerWin();
                        } else {
                            __dispTie();
                        }
                        break;
                    case WinLoss.Dealer:
                        __dispDealerWin();
                        break;
                    case WinLoss.Player:
                        __dispPlayerWin();
                        break;
                    default:
                        throw new ArgumentException("What the fuck happened? I didn't get a valid WinLoss out of checkWinLoss()");
                }
            }
        }

        private bool dealerTurn() {
            // TODO: verify
            if (dealer.Sum < 17 || (dealer.NumberOfAces > 0 && dealer.Sum <= 17)) {
                dealer.Draw();
                return true;
            }
            return false;
        }

        private void split() {
            if (turn == 1 && !playerSplit && player.CanSplit) {
                psplit = player.Split();
                playerSplit = true;
            }
        }

        private void double_down() {
            if (turn == 0) {
                player.Draw();
                player.Bet *= 2;
                playerDoubledDown = true;
            }
        }

        private int displayMenu() {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1) Hit");
            Console.WriteLine("2) Stand");
            Console.WriteLine("3) Split");
            Console.WriteLine("4) Double Down");
            //TODO Allow a doubling factor of 1.0 to 2.0

            int t = nextInt();
            if (t < 0 || t > 4) {
                Console.WriteLine("Invalid option. Choose 1-4.");
                return displayMenu();
            } else {
                return t;
            }
        }

        private int nextInt() {
            String c = Console.ReadLine();
            int x = -1;
            try {
                x = Int32.Parse(c);
            } catch {
                return -1;
            }
            return x;
        }
    }
}
