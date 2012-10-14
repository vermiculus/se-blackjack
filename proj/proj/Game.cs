﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    /// <summary>
    /// Contains program logic for playing multiple games of Blackjack on the console
    /// </summary>
    class Game {
        public static uint NUM_SHOES = 1;
        public static uint MIN_BET = 20;

        private Shoe shoe;
        PlayerHand player;
        DealerHand dealer;

        public bool GameOver {
            get {
                return player.Cash < MIN_BET;
            }
        }

        uint turn;

        private bool endTurns;

        public Game() {
            shoe = new Shoe(NUM_SHOES);
            player = new PlayerHand(shoe);
            dealer = new DealerHand(shoe);
            player.PutCardsBack();
            dealer.PutCardsBack();
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
            Console.WriteLine("\n Cash: {0,4:N0}  Bet: {1,3:N0}\n", player.Cash, player.Bet);
            if (endTurns) {
                Console.WriteLine("  Dealer's Hand: {0}", dealer.ToRevealingString());
            } else {
                Console.WriteLine("  Dealer's Hand: {0}", dealer.ToString());
            }
            Console.WriteLine("      Your Hand: {0}", player.ToString());
        }

        public void Play() {
            turn = 1;
            getBet();

            player.Draw(2);
            dealer.Draw(2);

            //printHands();

            // TODO: incorrect [from old source -- dunno what this means or why it's incorrect]
            BlackjackAction a;
            while (checkWinLoss() == WinLoss.NoWin && !endTurns) {
                a = displayMenu();
                switch (a) {
                    case BlackjackAction.Stand:
                        while (dealer.doTurn()) ;
                        endTurns = true;
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
                        break;
                    case BlackjackAction.EndGame:
                        quit();
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
            WinLoss ret = WinLoss.NoWin;
            if (player.IsBust && dealer.IsBust) {// TODO: this case || (player.IsPerfect && dealer.IsPerfect)) {
                endTurns = true;
                if (player.Sum < dealer.Sum) {
                    ret = WinLoss.Player;
                } else if (player.Sum > dealer.Sum)	{
                    ret = WinLoss.Dealer;
                } else {
                    ret = WinLoss.Tie;
                }
            }
            if (player.IsBust || dealer.IsPerfect) {
                endTurns = true;
                ret = WinLoss.Dealer;
            }
            if (dealer.IsBust || player.IsPerfect) {
                endTurns = true;
                ret = WinLoss.Player;
            }
            return ret;
        }

        private void __dispDealerWin() {
            Console.WriteLine("\n     You lost with {0} points against the dealer's hand of {1} points. :C", player.Sum, dealer.Sum);
        }
        private void __dispPlayerWin() {
            Console.WriteLine("\n     You won with {0} points against the dealer's hand of {1} points. :D", player.Sum, dealer.Sum);
        }
        private void __dispTie() {
            Console.WriteLine("\n     You tied the dealer with {0} points! :O", player.Sum);
        }

        private void end() {
            if (endTurns) {
                Console.Clear();
                Console.WriteLine();
                printHands();
                Console.WriteLine();
                WinLoss w = checkWinLoss();
                if (player.Sum == dealer.Sum) {
                    w = WinLoss.Tie;
                }
                switch (w) {
                    case WinLoss.NoWin: // TODO: There has to be a better way to do this
                        if (player.Sum > dealer.Sum) {
                            __dispPlayerWin();
                            w = WinLoss.Player; // TODO: Fix the checkWinLoss method to account for this.
                        } else {
                            __dispDealerWin();
                            w = WinLoss.Dealer;
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

                player.doBet(w);

                endTurns = false;

                player.PutCardsBack();
                dealer.PutCardsBack();
            }
        }

        private BlackjackAction displayMenu() {
            Console.Clear();
            printHands();
            Console.WriteLine("\n\n     What would you like to do?\n");
            Console.WriteLine(" [1] Hit");
            Console.WriteLine(" [2] Stand");
            Console.WriteLine(" [3] Split");
            Console.WriteLine(" [4] Double Down");
            Console.WriteLine(" [0] Quit Game");
            //TODO: Allow a doubling factor of 1.0 to 2.0 [what did I mean by this? - from old code]

            ConsoleKeyInfo k = Console.ReadKey(true);
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
        }
        public void getBet() {
            Console.Clear();
            Console.WriteLine("\n Cash: {0,4:N0}\n", player.Cash);
            Console.Write("\n How much would you like to bet on this game? (0 to quit)\n ");
            string input = Console.ReadLine();
            uint bet;
            if (!UInt32.TryParse(input, out bet)) {
                Console.WriteLine("\n Invalid. Please enter a number that's bigger than twenty.");
                Console.ReadKey(true);
                getBet();
            } else if (bet > player.Cash) {
                Console.WriteLine("\n Nice try. Bet only what you can pay up!! \n\n dirty stink...");
                Console.ReadKey(true);
                getBet();
            } else if (bet == 0) {
                quit();
            } else if (bet < MIN_BET) {
                Console.WriteLine("\n Stop wasting my time! Bet at least {0}. \n\n poop head...", MIN_BET);
                Console.ReadKey(true);
                getBet();
            } else {
                player.Bet = bet;
                player.Cash -= bet;
            }
        }
        private static void quit() {
            Console.Clear();
            Console.Write("\n Peace bro");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }
}
