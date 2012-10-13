using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    /// <summary>
    /// Contains program logic for playing multiple games of Blackjack on the console
    /// </summary>
    class Game {
        public static uint NUM_SHOES = 1;

        private Shoe shoe;
        PlayerHand player;
        DealerHand dealer;

        uint turn;

        private bool endTurns;

        public Game() {
            shoe = new Shoe(NUM_SHOES);
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
                Console.WriteLine("  Dealer's Hand: {0}", dealer.ToRevealingString());
            } else {
                Console.WriteLine("  Dealer's Hand: {0}", dealer.ToString());
            }
            Console.WriteLine("      Your Hand: {0}", player.ToString());
        }

        public void Play() {
            player = new PlayerHand(shoe);
            dealer = new DealerHand(shoe);

            turn = 1;
            printHands();

            // TODO: incorrect [from old source -- dunno what this means or why it's incorrect]
            BlackjackAction a;
            while (checkWinLoss() == WinLoss.NoWin && !endTurns) {
                a = displayMenu();
                switch (a) {
                    case BlackjackAction.Stand:
                        while (dealer.doTurn()) ;
                        endTurns = true;
                        printHands();
                        break;
                    case BlackjackAction.Split:
                        if (!player.CanSplit) {
                            Console.WriteLine("  You can't split now!");
                            Console.ReadKey(true);
                        } else {
                            player.doTurn(BlackjackAction.Split);
                            dealer.doTurn();
                        }
                        break;
                    case BlackjackAction.DoubleDown:
                        player.doTurn(BlackjackAction.DoubleDown);
                        while (dealer.doTurn()) ;
                        endTurns = true;
                        printHands();
                        break;
                    default:
                        player.doTurn(a);
                        dealer.doTurn();
                        break;
                }
            }
            end();
        }

        private WinLoss checkWinLoss() {
            // TODO: verify accuracy?
            if ((player.IsBust && dealer.IsBust) || (player.IsPerfect && dealer.IsPerfect)) {
                endTurns = true;
                return WinLoss.Tie;
            }
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
            Console.WriteLine("  You lost with {0} points against the dealer's hand of {1} points. :C", player.Sum, dealer.Sum);
        }

        private void __dispPlayerWin() {
            Console.WriteLine("  You won with {0} points against the dealer's hand of {1} points. :D", player.Sum, dealer.Sum);
        }

        private void __dispTie() {
            Console.WriteLine("  You tied the dealer with {0} points! :O", player.Sum);
        }

        private void end() {
            if (endTurns) {
                Console.Clear();
                printHands();
                Console.WriteLine();
                WinLoss w = checkWinLoss();
                switch (w) {
                    case WinLoss.NoWin: // TODO: There has to be a better way to do this
                        if (player.Sum > dealer.Sum) {
                            __dispPlayerWin();
                        } else {
                            __dispDealerWin();
                        }
                        break;
                    case WinLoss.Dealer:
                        __dispDealerWin();
                        break;
                    case WinLoss.Player:
                        __dispPlayerWin();
                        break;
                    case WinLoss.Tie:
                        __dispTie();
                        break;
                    default:
                        throw new ArgumentException("What the fuck happened? I didn't get a valid WinLoss out of checkWinLoss()");
                }

                endTurns = false;

                player.PutCardsBack();
                dealer.PutCardsBack();
            }
        }

        private BlackjackAction displayMenu() {
            Console.Clear();
            printHands();
            Console.WriteLine();
            Console.WriteLine("  What would you like to do?");
            Console.WriteLine(" [1] Hit");
            Console.WriteLine(" [2] Stand");
            Console.WriteLine(" [3] Split");
            Console.WriteLine(" [4] Double Down");
            Console.WriteLine(" [0] Quit Game");
            //TODO: Allow a doubling factor of 1.0 to 2.0 [what did I mean by this? - from old code]

            ConsoleKeyInfo k = Console.ReadKey(true); //nextInt();
            if (!Char.IsDigit(k.KeyChar)) {
                Console.WriteLine("  Invalid option. Choose 0-4.");
                return displayMenu();
            }
            int t = Int32.Parse("" + k.KeyChar);
            if (t < 0 || t > 4) {
                Console.WriteLine("  Invalid option. Choose 0-4.");
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
            Console.Clear();
        }
    }
}
