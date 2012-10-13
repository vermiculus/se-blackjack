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

        private Shoe shoe;
        PlayerHand player;
        DealerHand dealer;

        int turn;

        private bool endTurns;

        public Game() {
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

        private void printHands() {
            if (endTurns) {
                Console.WriteLine("Dealer's Hand: {0}", dealer.ToRevealingString());
            } else {
                Console.WriteLine("Dealer's Hand: {0}", dealer.ToString());
            }
            if (player.HasSplit) {
                Console.WriteLine("Your Hand (1): {0}", player.ToString());
                Console.WriteLine("          (2): {0}", player.SplitHand.ToString());
            } else {
                Console.WriteLine("    Your Hand: {0}", player.ToString());
            }
        }

        public void Play() {
            player = new PlayerHand(shoe);
            dealer = new DealerHand(shoe);

            turn = 1;
            printHands();

            // TODO: incorrect [from old source -- dunno what this means]
            BlackjackAction a;
            while (checkWinLoss() == WinLoss.NoWin && !endTurns) {
                a = displayMenu();
                switch (a) {
                    case BlackjackAction.Stand:
                        while (dealer.doTurn(BlackjackAction.None));
                        endTurns = true;
                        printHands();
                        break;
                    case BlackjackAction.Split:
                        if (!player.CanSplit) {
                            Console.WriteLine("You can't split now!");
                            continue;
                        }
                        break;
                    default:
                        player.doTurn(a);
                        dealer.doTurn(BlackjackAction.None);
                        break;
                }

                if (!endTurns) {
                    printHands();
                    turn++;
                }
            }
            end();
        }

        private WinLoss checkWinLoss() {
            // TODO: verify accuracy?
            if (player.IsBust || dealer.IsPerfect) {
                endTurns = true;
                return WinLoss.Dealer;
            }
            if (dealer.IsBust || player.IsPerfect) {
                endTurns = true;
                return WinLoss.Player;
            }
            return WinLoss.NoWin;
        }

        private void __dispDealerWin() {
            Console.WriteLine("You lost with {0} points against the dealer's hand of {1} points. :[", player.Sum, dealer.Sum);
        }

        private void __dispPlayerWin() {
            Console.WriteLine("You won with {0} points against the dealer's hand of {1} points. :D", player.Sum, dealer.Sum);
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

                printHands();
                endTurns = false;

                player.PutCardsBack();
                dealer.PutCardsBack();
            }
        }

        private void split() {
        }

        private void double_down() {
        }

        private BlackjackAction displayMenu() {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1] Hit");
            Console.WriteLine("2] Stand");
            Console.WriteLine("3] Split");
            Console.WriteLine("4] Double Down");
            Console.WriteLine("0] Quit Game");
            //TODO Allow a doubling factor of 1.0 to 2.0

            ConsoleKeyInfo k = Console.ReadKey(true); //nextInt();
            if (!Char.IsDigit(k.KeyChar)) {
                Console.WriteLine("Invalid option. Choose 0-4.");
                return displayMenu();
            }
            int t = Int32.Parse(""+k.KeyChar);
            if (t < 0 || t > 4) {
                Console.WriteLine("Invalid option. Choose 0-4.");
                return displayMenu();
            } else {
                switch (t) {
                    case 0:
                        return BlackjackAction.EndGame;
                    case 1:
                        return BlackjackAction.Hit;
                    case 2:
                        return BlackjackAction.Stand;
                    case 3:
                        return BlackjackAction.Split;
                    case 4:
                        return BlackjackAction.DoubleDown;
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
