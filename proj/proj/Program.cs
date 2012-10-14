using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    class Program {
        static void Main(string[] args) {
            Game g = new Game();
            while (!g.GameOver) {
                g.Play();
                Console.WriteLine("\n\n Game ended.");
                Console.ReadKey(true);
                Console.Clear();
            }
            Console.WriteLine("\n You ran out of cash. You should feel bad.\n" +
                              "\n Now you have to go back home to your wife" +
                              "\n and explain why she can't buy groceries" +
                              "\n for your family for TWO MONTHS because" +
                              "\n you just HAD to go out gambling." +
                              "\n\n Asshole.");
            Console.ReadKey(true);
        }
    }
}
