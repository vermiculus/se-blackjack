using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    class Game {
        const int NUM_SHOES = 1;

        private enum WinLoss {
            NoWin,
            Dealer,
            Player
        }

        private enum Action {
            Hit,
            Stand,
            Split,
            DoubleDown
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
            shoe = new Shoe(NUM_SHOES);
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
            Action c;
            if (playerDoubledDown) // TODO: Is this functionality required anymore?
                c = Action.DoubleDown;
            else
                c = displayMenu();

            switch (c) {
                case Action.Hit:
                    hit();
                    break;
                case Action.Stand:
                    stand();
                    break;
                case Action.Split:
                    split();
                    break;
                case Action.DoubleDown:
                    double_down();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("What?");
            }
            if (!endTurns) {
                printHands();
                turn++;
            }
        }

        private void printHands(bool displayDealer = false) {
            if (displayDealer) {
                //  TODO: not implemented
                Console.WriteLine("Dealer's Hand: {0}", dealer.ToDealerString());
            } else {
                Console.WriteLine("Dealer's Hand: {0}", dealer.ToString());
            }
            if (playerSplit) {
                Console.WriteLine("Your Hand (1): {0}", player.ToString());
                Console.WriteLine("          (2): {0}", psplit.ToString());
            } else {
                Console.WriteLine("    Your Hand: {0}", player.ToString());
            }
        }

        public void Play() {
            player = new BlackjackHand(shoe);
            dealer = new BlackjackHand(shoe);

            turn = 1;
            playerSplit = false;
            printHands();

            // TODO: incorrect [from old source -- dunno what this means]
            while (checkWinLoss() == WinLoss.NoWin && !endTurns) {
                doTurn();
            }

            end();
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
            dealerTurn();
        }

        private void stand() {
            while (dealerTurn()) ;

            printHands(true);
            endTurns = true;
        }

        private void __dispDealerWin() {
            Console.WriteLine("You lost with {0} points under the dealer's hand of {1} points. :[", player.Sum, dealer.Sum);
        }

        private void __dispPlayerWin() {
            Console.WriteLine("You won with {0} points over the dealer's hand of {1} points. :D", player.Sum, dealer.Sum);
        }

        private void __dispTie() {
            Console.WriteLine("You tied the dealer with {0} points! :O", player.Sum);
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
                endTurns = false;
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

        private Action displayMenu() {
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
                switch (t) {
                    case 1:
                        return Action.Hit;
                    case 2:
                        return Action.Stand;
                    case 3:
                        return Action.Split;
                    case 4:
                        return Action.DoubleDown;
                    default:
                        throw new ArgumentOutOfRangeException("Wha happun?");
                };
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
