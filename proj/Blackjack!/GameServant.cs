using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Blackjack {
    public class GameServant : INotifyPropertyChanged {
        public enum ActiveHandPotentials {
            Normal,
            Split,
            None
        }

        [Serializable]
        public class IllegalMoveException : Exception {
            public IllegalMoveException() {
            }
            public IllegalMoveException(string message) : base(message) {
            }
            public IllegalMoveException(string message, Exception inner) : base(message, inner) {
            }
            protected IllegalMoveException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) {
            }
        }

        [Serializable]
        public class BustedException : Exception {
            public BustedException() {
            }
            public BustedException(string message) : base(message) {
            }
            public BustedException(string message, Exception inner) : base(message, inner) {
            }
            protected BustedException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) {
            }
        }

        [Serializable]
        public class BlackjackException : Exception {
            public BlackjackException() {
            }
            public BlackjackException(string message) : base(message) {
            }
            public BlackjackException(string message, Exception inner) : base(message, inner) {
            }
            protected BlackjackException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) {
            }
        }

        #region Instance Variables
        #region Front-matter
        private string _playerName;
        private uint _numWins;

        public uint NumWins {
            get {
                return _numWins;
            }
        }
        private uint _numLosses;

        public uint NumLosses {
            get {
                return _numLosses;
            }
        }
        private uint _largestWin;

        public uint LargestWin {
            get {
                return _largestWin;
            }
        }
        private uint _largestLoss;

        public uint LargestLoss {
            get {
                return _largestLoss;
            }
        }
        #endregion

        #region Back-matter
        public static uint NUM_DECKS = 1;
        public static uint MIN_BET = 20;
        private CardCollection _source;
        private CardCollection _discard;

        private PlayerHand _playerHand;

        internal PlayerHand PlayerHand {
            get {
                return _playerHand;
            }
        }
        private ActiveHandPotentials _activeHand;
        private DealerHand _dealerHand;
        internal DealerHand DealerHand {
            get {
                return _dealerHand;
            }
        }
        #endregion
        #endregion

        public GameServant() {
            this.ActiveHand = ActiveHandPotentials.None;
            this._source = new CardCollection(1);
            this._discard = new CardCollection(1, false);

            this.ActiveHand = ActiveHandPotentials.Normal;
            this._playerHand = new PlayerHand(_source, _discard);
            this._dealerHand = new DealerHand(_source, _discard);
            (new GetUserName(this)).ShowDialog();
        }

        public void Hit() {
            switch (ActiveHand) {
                case ActiveHandPotentials.Normal:
                    PlayerHand.Draw();
                    if (PlayerHand.IsBust) {
                        throw new BustedException();
                    }
                    break;
                case ActiveHandPotentials.Split:
                    PlayerHand.SplitHand.Draw();
                    break;
                case ActiveHandPotentials.None:
                default:
                    throw new Exception("Oh no! What happened? I don't even know!");
            }
            NotifyAll();
        }

        public void Split() {
            switch (ActiveHand) {
                case ActiveHandPotentials.Normal:
                    PlayerHand.Split();
                    break;
                case ActiveHandPotentials.Split:
                case ActiveHandPotentials.None:
                default:
                    throw new IllegalMoveException("Oh no! I can't split like this!");
            }
            NotifyAll();
        }

        public void Stand() {
            switch (_activeHand) {
                case ActiveHandPotentials.Normal:
                    PlayerHand.HasStood = true;
                    break;
                case ActiveHandPotentials.Split:
                    PlayerHand.SplitHand.HasStood = true;
                    break;
                case ActiveHandPotentials.None:
                default:
                    throw new IllegalMoveException("wait... what?");
            }
            if (PlayerHand.HasSplit) {
                if (ActiveHand == ActiveHandPotentials.Split) {
                    while (DealerHand.Sum <= 17) {
                        DealerHand.Draw();
                    }
                    _displayHole = true;
                }
            } else {
                while (DealerHand.Sum <= 17) {
                    DealerHand.Draw();
                }
                _displayHole = true;
            }
        }

        /// <summary>
        /// Compares 'on' to the dealer's hand for win/loss and does any needed action with the player's cash/bet
        /// </summary>
        /// <param name="on">a playerhand to compare against</param>
        /// <returns>Who won the round, if either.</returns>
        public WinLoss Winner(PlayerHand on) {
            WinLoss ret = WinLoss.NoWin;
            if (on.IsBlackjack && DealerHand.IsBlackjack) {
                ret = WinLoss.Push;
            } else if (on.IsBust && DealerHand.IsBust) {
                if (on.Sum < DealerHand.Sum) {
                    ret = WinLoss.Player;
                } else if (on.Sum > DealerHand.Sum) {
                    ret = WinLoss.Dealer;
                } else {
                    ret = WinLoss.Push;
                }
            } else if (on.IsBust || DealerHand.IsBlackjack) {
                ret = WinLoss.Dealer;
            } else if (DealerHand.IsBust || on.IsBlackjack) {
                ret = WinLoss.Player;
            } else {
                if (PlayerHand.Sum > DealerHand.Sum) {
                    ret = WinLoss.Player;
                } else if (PlayerHand.Sum < DealerHand.Sum) {
                    ret = WinLoss.Dealer;
                } else {
                    ret = WinLoss.Push;
                }
            }

            switch (ret) {
                case WinLoss.Dealer:
                    _numLosses++;
                    _largestLoss = PlayerHand.Bet > _largestLoss ? PlayerHand.Bet : _largestLoss;
                    break;
                case WinLoss.Player:
                    _numWins++;
                    _largestWin = PlayerHand.Bet > _largestWin ? PlayerHand.Bet : _largestWin;
                    break;
                default:
                    break;
            }

            if (ret != WinLoss.NoWin) {
                on.doBet(ret);
            }

            return ret;
        }

        #region Properties and Events
        public ActiveHandPotentials ActiveHand {
            get {
                return _activeHand;
            }
            set {
                _activeHand = value;
                Notify("ActiveHand");     // The active hand has changed,
                Notify("IsNormalActive"); // and thus so have these
                Notify("IsSplitActive");  // two properties (used to control which button group is enabled)
            }
        }

        public bool IsNormalActive {
            get {
                return ActiveHand == ActiveHandPotentials.Normal;
            }
        }
        public bool IsSplitActive {
            get {
                return ActiveHand == ActiveHandPotentials.Split;
            }
        }
        public string PlayerName {
            get {
                return _playerName;
            }
            set {
                _playerName = value;
                Notify("PlayerName");
            }
        }
        public uint PlayerFunds {
            get {
                return _playerHand != null ? _playerHand.Cash : 500;
            }
        }
        public string PlayerNormalBet {
            get {
                return _playerHand.Bet.ToString();
            }
        }
        public string PlayerSplitBet {
            get {
                return _playerHand.HasSplit ? _playerHand.SplitHand.Bet.ToString() : "";
            }
        }
        public bool CanPlayerSplit {
            get {
                return PlayerHand.CanSplit;
            }
        }
        public List<Card> PlayerNormalCards {
            get {
                return _playerHand.Cards;
            }
        }
        public List<Card> PlayerSplitCards {
            get {
                return _playerHand.SplitHand.Cards;
            }
        }
        public WinLoss PlayerNormalWinLoss {
            get {
                return Winner(PlayerHand);
            }
        }
        public WinLoss PlayerSplitWinLoss {
            get {
                return Winner(PlayerHand.SplitHand);
            }
        }
        public bool IsOver {
            get {
                if (PlayerHand.HasSplit) {
                    return (PlayerHand.HasStood || PlayerHand.IsBust) && (PlayerHand.SplitHand.HasStood || PlayerHand.SplitHand.IsBust);
                } else {
                    return PlayerHand.HasStood || PlayerHand.IsBust;
                }
            }
        }


        string[] all = { "IsNormalActive",
                             "IsSplitActive",
                             "PlayerName",
                             "PlayerFunds",
                             "PlayerNormalBet",
                             "PlayerSplitBet",
                             "CanPlayerSplit",
                             "PlayerNormalCards",
                             "PlayerSplitCards",
                             "ActiveHand",
                             "PlayerNormalWinLoss",
                             "PlayerSplitWinLoss"
                           };
        private void NotifyAll() {
            foreach (string prop in all) {
                Notify(prop);
            }
        }
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _displayHole;

        public bool DisplayHole {
            get {
                return _displayHole;
            }
        }

        // This method is called by the Set accessor of each property.
        // parameter causes the property name of the caller to be substituted as an argument.
        void Notify(string propName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
        #endregion
        
        internal void NewRound() {
            _playerHand.DiscardAll();
            _dealerHand.DiscardAll();
            _displayHole = false;
            ActiveHand = ActiveHandPotentials.Normal;

            if (PlayerHand.Cash < MIN_BET) {
                throw new IllegalMoveException("I can't do that, Dave.");
            }
            var b = new GetBet(this);
            b.ShowDialog();
            PlayerHand.makeBet(b.Bet);
            b.Close();
            _playerHand.Draw(2);
            _dealerHand.Draw(2);

            if (_playerHand.IsBlackjack) {
                throw new BlackjackException();
            }

            NotifyAll();
        }

        public WinLoss?[] EndRound() {
            ActiveHand = ActiveHandPotentials.None;
            WinLoss Hand1 = Winner(PlayerHand);
            WinLoss? Hand2 = null;
            if (PlayerHand.HasSplit) {
                Hand2 = Winner(PlayerHand.SplitHand);
            }
            WinLoss?[] ret = {Hand1, Hand2};
            return ret;
        }
    }
}
