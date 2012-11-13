using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack {
    class Program {
        static void Main2(string[] args) {
            Game g = new Game();
            while (!g.GameOver && g.ContinuePlay) {
                g.Play();
                Console.WriteLine("\n\n Game ended.");
                Console.ReadKey(true);
                Console.Clear();
            }

            switch (g.State) {
                case Game.GameState.CasinoOwner:
                    Console.WriteLine("\n GET OUT. GET OUT OF THIS PLACE. LEAVE, YOU DIRTY," +
                                      "\n ROTTEN, CHEATER. Probably hiding cards under your sleeve," +
                                      "\n or something else equally evil. You expect US to believe" +
                                      "\n that someone can get this 'lucky'? Bull." +
                                      "\n\n\n\n Bull.");
                    break;
                case Game.GameState.CardCounter:
                    Console.WriteLine("\n [You get up from the table, and all eyes are on you.]\n" +
                                      "\n \"What?\" you say, as jealous men and women stare down" +
                                      "\n the cause of their demise. The dealer makes a phone call" +
                                      "\n and you decide it's better to leave sooner rather than later.");
                    break;
                case Game.GameState.Rich:
                    Console.WriteLine("\n Wow. Just wow.\n" +
                                      "\n Good for you, buddy. Buy your friend a drink?" +
                                      "\n Hell, buy everyone drinks. Right?");
                    break;
                case Game.GameState.Over:
                    Console.WriteLine("\n Well done! You came out with more money" +
                                      "\n than you put in! Good thinking to quit" +
                                      "\n while you're ahead.");
                    break;
                case Game.GameState.Even:
                    Console.WriteLine("\n Well, you managed to break even.\n" +
                                      "\n Go home to your wife and kids, and try" +
                                      "\n not to gamble anymore with grocery money" +
                                      "\n unless you can actually play Blackjack.");
                    break;
                case Game.GameState.Under:
                    Console.WriteLine("\n You lost money! Quick! - to the slots!\n" +
                                      "\n On second thought, don't. Baby Billy needs" +
                                      "\n groceries, you selfish bum.");
                    break;
                case Game.GameState.Broke:
                    Console.WriteLine("\n You ran out of cash. You should feel bad.\n" +
                                      "\n Now you have to go back home to your wife" +
                                      "\n and explain why she can't buy groceries" +
                                      "\n for your family for TWO MONTHS because" +
                                      "\n you just HAD to go out gambling.\n" +
                                      "\n Asshole.");
                    break;
                default:
                    break;
            }

            Console.WriteLine("\n\n");
            Console.WriteLine(" Wins:Losses => {0}:{1}", g.NumWins, g.NumLosses);
            Console.WriteLine(" Largest win => {0}", g.LargestWin);
            Console.WriteLine(" Largest loss => {0}", g.LargestLoss);

            Console.ReadKey(true);
        }
    }
}
