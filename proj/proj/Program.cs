using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    class Program {
        static void Main(string[] args) {
            Game g = new Game();
            while (true) {
                g.Play();
                Console.WriteLine("\n\n Game ended.");
                Console.ReadKey(true);
                Console.Clear();
            }
        }
    }
}
