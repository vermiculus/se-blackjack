using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    public enum BlackjackAction {
        /// <summary>
        /// The player and dealer both take a card (in that order)
        /// </summary>
        Hit,
        Stand,
        Split,
        DoubleDown,
        EndGame,
        None
    }
}
