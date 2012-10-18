﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test {
    /// <summary>
    /// Denotes and describes any action a single turn can produce.
    /// </summary>
    /// <remarks>
    /// For the dealer, the action is always 'None' (since it's algorithmic)
    /// </remarks>
    public enum BlackjackAction {
        /// <summary>
        /// The player and dealer both take a card (in that order)
        /// </summary>
        Hit,

        /// <summary>
        /// The player ends his turn and the dealer takes as many cards as allowed.
        /// </summary>
        Stand,

        /// <summary>
        /// The player's hand becomes two.
        /// </summary>
        /// <remarks>
        /// This is only a valid move is the player's hand has exactly two cards of equal rank.
        /// </remarks>
        Split,

        /// <summary>
        /// The bet is doubled and the player stands.
        /// </summary>
        DoubleDown,

        /// <summary>
        /// The player has chosen to end the game.
        /// </summary>
        EndGame,

        /// <summary>
        /// No particular action is necessary.
        /// </summary>
        /// <remarks>
        /// Only the dealer should use this action.
        /// </remarks>
        None
    }
}
