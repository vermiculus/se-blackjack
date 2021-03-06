﻿//#define TESTING_MODE

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Blackjack {
    /// <summary>
    /// Contains program logic for playing multiple games of Blackjack on the console. Salted for completion by 30 October 2012 with a dash of peppermint
    /// </summary>
    class Game
    {
        #region "Properties and Fields"
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

        private uint num_wins;
        private uint num_losses;
        private uint largest_win;
        private uint largest_loss;

        public uint NumWins {
            get { return num_wins; }
        }
        public uint NumLosses {
            get { return num_losses; }
        }
        public uint LargestWin {
            get { return largest_win; }
        }
        public uint LargestLoss {
            get { return largest_loss; }
        }

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


#if TESTING_MODE
        private PredeterministicCardCollection source;
        private CardCollection discard;
#else
        private CardCollection source;
        private CardCollection discard;
#endif
        PlayerHand player;
        DealerHand dealer;

        public bool GameOver {
            get { return player.Cash < MIN_BET; }
        }
        #endregion

        public Game() {
#if TESTING_MODE
            var __path = @"../../cards.txt";
            if (!System.IO.File.Exists(__path))
            {
                Console.WriteLine("\"{0}\" does not exist.", __path);
                Console.ReadKey(true);
                System.Environment.Exit(0);
            }
            
            source = new PredeterministicCardCollection(__path);
            
            discard = new CardCollection(NUM_DECKS, false);
#else
            source = new CardCollection(NUM_DECKS);
            discard = new CardCollection(NUM_DECKS, false);
#endif
            player = new PlayerHand(source, discard);
            dealer = new DealerHand(source, discard);

            num_wins = 0;
            num_losses = 0;
            largest_win = UInt32.MinValue;
            largest_loss = UInt32.MinValue;

            playAgain = true;
        }

        public void Play() {
            Action quitGame = () => playAgain = false;

            if (!getBet()) {
                quitGame();
                return;
            }

            try
            {
                player.Draw(2);
                dealer.Draw(2);
            }
            catch (OutOfCardsException e)
            {
                Console.Clear();
                Console.WriteLine("\n For some reason, the system ran out of cards: \"{0}\"", e.Message);
                Console.ReadKey(true);
                Environment.Exit(0);
            }


            player.makeTurns(dealer.Top, quitGame);
            if (ContinuePlay) {
                switch (checkWinLoss()) {
                    case WinLoss.NoWin:
                        dealer.doTurn();
                        end();
                        break;
                    case WinLoss.Dealer:
                    case WinLoss.Player:
                    case WinLoss.Push:
                        end();
                        break;
                    default:
                        throw new InvalidOperationException("What?");
                }
            }
            reset();
        }

        private void reset()
        {
#if TESTING_MODE
            player = new PlayerHand(source, discard, player.Cash);
            dealer = new DealerHand(source, discard);
#else
            player.PutCardsBack();
            dealer.PutCardsBack();
#endif
        }

        private WinLoss checkWinLoss(bool onSplit = false) {
            // TODO: verify accuracy?
            PlayerHand p = onSplit ? player.SplitHand : player;
            WinLoss ret = WinLoss.NoWin;
            if (p.IsBlackjack && dealer.IsBlackjack)
            {
                ret = WinLoss.Push;
            } else if (p.IsBust && dealer.IsBust) {
                if (p.Sum < dealer.Sum) {
                    ret = WinLoss.Player;
                } else if (p.Sum > dealer.Sum) {
                    ret = WinLoss.Dealer;
                } else {
                    ret = WinLoss.Push;
                }
            } else if (p.IsBust || dealer.IsBlackjack) {
                ret = WinLoss.Dealer;
            } else if (dealer.IsBust || p.IsBlackjack) {
                ret = WinLoss.Player;
            }
            return ret;
        }

        private void end(bool onSplit = false) {
            Action dispDW = () => Console.WriteLine("\n     You lost! :C");
            Action dispPW = () => Console.WriteLine("\n     You won! :D");

            PlayerHand p = onSplit ? player.SplitHand : player;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(" Cash: {0,4:N0}  Bet: {1,3:N0}\n", player.Cash, p.Bet);
            Console.WriteLine("  Dealer's Hand: {0}", dealer.ToRevealingString());
            Console.WriteLine("      Your Hand: {0}", p.ToString());
            Console.WriteLine();

            WinLoss w = checkWinLoss(onSplit);
            switch (w) {
                case WinLoss.NoWin:
                    throw new Exception("How did I get here? I should only enter 'end' at the end of a game, but WinLoss.NoWin is reserved for the continuation of a game!");
                case WinLoss.Dealer:
                    dispDW();
                    break;
                case WinLoss.Player:
                    dispPW();
                    break;
                case WinLoss.Push:
                    Console.WriteLine("\n     You tied the dealer with {0} points! :O", player.Sum);
                    break;
                default:
                    throw new ArgumentException("What happened? I didn't get a valid WinLoss out of checkWinLoss()");
            }

            switch (w) {
                case WinLoss.Dealer:
                    num_losses++;
                    if (player.Bet > largest_loss) {
                        largest_loss = player.Bet;
                    }
                    break;
                case WinLoss.Player:
                    num_wins++;
                    if (player.Bet > largest_win) {
                        largest_win = player.Bet;
                    }
                    break;
                case WinLoss.Push:
                    break;
                default:
                    break;
            }

            p.doBet(w);
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
