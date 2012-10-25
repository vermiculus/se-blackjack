using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace test {
    /// <summary>
    /// Contains program logic for playing multiple games of Blackjack on the console. Salted for completion by 30 October 2012
    /// </summary>
    class Game {

        public enum GameState {
            CasinoOwner,
            CardCounter,
            Rich,
            Over,
            Under,
            Even,
            Broke
        }

        public static uint NUM_DECKS = 1;
        public static uint MIN_BET = 1;

        private bool playAgain;

        internal GameState State {
            get {
                if (player.Cash > PlayerHand.DEFAULT_CASH * 1000) {
                    return GameState.CasinoOwner;
                } else if (player.Cash > PlayerHand.DEFAULT_CASH * 100) {
                    return GameState.CardCounter;
                } else if (player.Cash > PlayerHand.DEFAULT_CASH * 10) {
                    return GameState.Rich;
                } else if (player.Cash > PlayerHand.DEFAULT_CASH) {
                    return GameState.Over;
                } else if (player.Cash == 0) {
                    return GameState.Broke;
                } else if (player.Cash == PlayerHand.DEFAULT_CASH) {
                    return GameState.Even;
                } else {
                    return GameState.Under;
                }
            }
        }

        public bool ContinuePlay {
            get { return playAgain; }
        }



        private CardCollection source;
        private CardCollection discard;
        PlayerHand player;
        DealerHand dealer;

        public bool GameOver {
            get { return player.Cash < MIN_BET; }
        }

        private bool endTurns;

        public Game() {
            source = new CardCollection(NUM_DECKS);
            discard = new CardCollection(NUM_DECKS, false);
            player = new PlayerHand(source, discard);
            dealer = new DealerHand(source, discard);
            playAgain = true;
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
            if (!getBet()) return;

            player.Draw(2);
            dealer.Draw(2);

            BlackjackAction a, b;

            player.makeTurns();

            while (checkWinLoss() == WinLoss.NoWin && playAgain) {
                bool again;
                if (!player.HasSplit) {
                    do {
                        again = false;
                        if (player.Sum < 21) {
                            a = displayMenu();
                        } else {
                            a = BlackjackAction.Stand;
                        }
                        switch (a) {
                            case BlackjackAction.Hit:
                                player.doTurn(a);
                                dealer.doTurn();
                                break;
                            case BlackjackAction.Stand:
                                while (dealer.doTurn()) ;
                                endTurns = true;
                                break;
                            case BlackjackAction.Split:
                                if (!player.CanSplit || player.HasSplit) {
                                    Console.WriteLine("  You can't split now!");
                                    Console.ReadKey(true);
                                    again = true;
                                } else {
                                    player.doTurn(BlackjackAction.Split);
                                    dealer.doTurn();
                                }
                                break;
                            case BlackjackAction.EndGame:
                                playAgain = false;
                                break;
                        }
                        if (endTurns) {
                            end();
                        }
                    } while (again);
                } else {
                    do {
                        if (player.Sum < 21) {
                            a = displayMenu(0);
                        } else {
                            a = BlackjackAction.Stand;
                        }
                        again = false;
                        switch (a) {
                            case BlackjackAction.Hit:
                                player.doTurn(a);
                                dealer.doTurn();
                                break;
                            case BlackjackAction.Stand:
                                while (dealer.doTurn()) ;
                                endTurns = true;
                                break;
                            case BlackjackAction.Split:
                                Console.WriteLine("  You can't split now!");
                                Console.ReadKey(true);
                                again = true;
                                break;
                            case BlackjackAction.EndGame:
                                playAgain = false;
                                break;
                        }
                        if (endTurns) {
                            end();
                        }

                        if (player.Sum < 21) {
                            b = displayMenu(1);
                        } else {
                            b = BlackjackAction.Stand;
                        }
                        switch (b) {
                            case BlackjackAction.Hit:
                                player.doTurn(b);
                                dealer.doTurn();
                                break;
                            case BlackjackAction.Stand:
                                while (dealer.doTurn()) ;
                                endTurns = true;
                                break;
                            case BlackjackAction.Split:
                                Console.WriteLine("  You can't split now!");
                                Console.ReadKey(true);
                                again = true;
                                break;
                            case BlackjackAction.EndGame:
                                playAgain = false;
                                break;
                        }
                        if (endTurns) {
                            end();
                        }
                    } while (again);
                }
            }
            endTurns = false;
        }

        private WinLoss checkWinLoss() {
            // TODO: verify accuracy?
            WinLoss ret = WinLoss.NoWin;
            if (player.IsBust && dealer.IsBust || player.IsBlackjack && dealer.IsBlackjack) {
                endTurns = true;
                if (player.Sum < dealer.Sum) {
                    ret = WinLoss.Player;
                } else if (player.Sum > dealer.Sum) {
                    ret = WinLoss.Dealer;
                } else {
                    ret = WinLoss.Push;
                }
            } else if (player.IsBust || dealer.IsBlackjack) {
                endTurns = true;
                ret = WinLoss.Dealer;
            } else if (dealer.IsBust || player.IsBlackjack) {
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
            Console.Clear();
            Console.WriteLine("");
            printHands();
            Console.WriteLine("");
            WinLoss w = checkWinLoss();
            if (player.Sum == dealer.Sum) {
                w = WinLoss.Push;
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
                case WinLoss.Push:
                    __dispTie();
                    break;
                default:
                    throw new ArgumentException("What the fuck happened? I didn't get a valid WinLoss out of checkWinLoss()");
            }

            player.doBet(w);

            player.PutCardsBack();
            dealer.PutCardsBack();
        }


        public bool getBet() {
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
                playAgain = false;
            } else if (bet < MIN_BET) {
                Console.WriteLine("\n Stop wasting my time! Bet at least {0}. \n\n poop head...", MIN_BET);
                Console.ReadKey(true);
                getBet();
            } else {
                player.makeBet(bet);
            }
            return playAgain;
        }
    }
}
