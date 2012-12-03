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
            public IllegalMoveException(string message)
                : base(message) {
            }
            public IllegalMoveException(string message, Exception inner)
                : base(message, inner) {
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
            public BustedException(string message)
                : base(message) {
            }
            public BustedException(string message, Exception inner)
                : base(message, inner) {
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
            public BlackjackException(string message)
                : base(message) {
            }
            public BlackjackException(string message, Exception inner)
                : base(message, inner) {
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
        public List<string> log;
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

        public void l(string s = "", bool printstats = true) {
            var d = DateTime.Now;
            log.Add(string.Format("{0}.{1}.{2}.{3}: {4}",
                d.Hour,
                d.Minute,
                d.Second,
                d.Millisecond,
                s));
            if (printstats) {
                log.Add(string.Format("\tDealer:{0}",
                    DealerHand.ToRevealingString()));
                log.Add(string.Format("\tPlayer:{0}{1}",
                    PlayerHand.ToString(),
                    PlayerHand.HasSplit ? PlayerHand.SplitHand.ToString() : ""));
                log.Add(string.Format("\tBet: {0}",
                    PlayerHand.Bet));
                log.Add(string.Format("\tCash: {0}",
                    PlayerHand.Cash));
                log.Add(string.Format("\tSource Pile: {0}",
                    PlayerHand.SourceCollection.ToString()));
                log.Add(string.Format("\tDiscard Pile: {0}",
                    PlayerHand.DiscardCollection.ToString()));
            }
        }

        public GameServant(bool testing = false) {
            log = new List<string>();
            l(string.Format("New Game ({0}:{1}:{2})", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), false);
            if (!testing) {
                l("In production mode...", false);
                this.ActiveHand = ActiveHandPotentials.None;
                l("Creating Source Deck", false);
                this._source = new CardCollection(1);
                l("Creating Discard Deck", false);
                this._discard = new CardCollection(1, false);

                this.ActiveHand = ActiveHandPotentials.Normal;

                l("Creating the player's hand", false);
                this._playerHand = new PlayerHand(_source, _discard);
                l("Creating the dealer's hand", false);
                this._dealerHand = new DealerHand(_source, _discard);
                l("Getting user name", false);
                (new GetUserName(this)).ShowDialog();
                l("User name received", false);
            } else {
                l("In testing mode...", false);
                this.ActiveHand = ActiveHandPotentials.None;
                var d = new Microsoft.Win32.OpenFileDialog();
                d.Filter = "CSV Files (.csv)|*.csv";
                d.ShowDialog();
                this._source = new PredeterministicCardCollection(d.FileName);
                this._discard = new CardCollection(Int32.MaxValue, false);

                this.ActiveHand = ActiveHandPotentials.Normal;
                this._playerHand = new PlayerHand(_source, _discard);
                this._dealerHand = new DealerHand(_source, _discard);
                (new GetUserName(this)).ShowDialog();
            }
            NotifyAll();
        }

        public void Hit() {
            l("Player hit", false);
            switch (ActiveHand) {
                case ActiveHandPotentials.Normal:
                    l("Normal hand; Player drawing", false);
                    PlayerHand.Draw();
                    if (PlayerHand.IsBust) {
                        l("Hand busted");
                        throw new BustedException();
                    }
                    l("Hit successful");
                    break;
                case ActiveHandPotentials.Split:
                    l("Split hand; Player drawing", false);
                    PlayerHand.SplitHand.Draw();
                    if (PlayerHand.SplitHand.IsBust) {
                        l("Hand busted");
                        throw new BustedException();
                    }
                    l("Hit successful");
                    break;
                case ActiveHandPotentials.None:
                default:
                    throw new Exception("Oh no! What happened? I don't even know!");
            }
            NotifyAll();
        }

        public void Split() {
            l("Player split", false);
            switch (ActiveHand) {
                case ActiveHandPotentials.Normal:
                    PlayerHand.Split();
                    l("Split successful");
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
                    l("Normal hand; player stood");
                    PlayerHand.HasStood = true;
                    break;
                case ActiveHandPotentials.Split:
                    l("Split hand; player stood");
                    PlayerHand.SplitHand.HasStood = true;
                    break;
                case ActiveHandPotentials.None:
                default:
                    throw new IllegalMoveException("wait... what?");
            }
            if (PlayerHand.HasSplit) {
                if (ActiveHand == ActiveHandPotentials.Split) {
                    l("Dealer turn begin", false);
                    while (DealerHand.Sum < 17) {
                        DealerHand.Draw();
                    }
                    _displayHole = true;
                    l("Dealer turn end");
                }
            } else {
                l("Dealer turn begin", false);
                while (DealerHand.Sum < 17) {
                    DealerHand.Draw();
                }
                _displayHole = true;
                l("Dealer turn end");
            }
        }

        /// <summary>
        /// Compares 'on' to the dealer's hand for win/loss and does any needed action with the player's cash/bet
        /// </summary>
        /// <param name="on">a playerhand to compare against</param>
        /// <returns>Who won the round, if either.</returns>
        public WinLoss Winner(PlayerHand on) {
            l("Checking Win/Loss");
            WinLoss ret = WinLoss.NoWin;
            if (on.IsBlackjack && DealerHand.IsBlackjack) {
                l("Both hands blackjack: push");
                ret = WinLoss.Push;
            } else if (on.IsBust && DealerHand.IsBust) {
                l(String.Format("Both hands bust. Hand Sum: {0} Dealer Sum: {1}", on.Sum, DealerHand.Sum));
                if (on.Sum < DealerHand.Sum) {
                    l("Player wins");
                    ret = WinLoss.Player;
                } else if (on.Sum > DealerHand.Sum) {
                    l("Dealer wins");
                    ret = WinLoss.Dealer;
                } else {
                    l("Push");
                    ret = WinLoss.Push;
                }
            } else if (on.IsBust || DealerHand.IsBlackjack) {
                l("Hand busted or Dealer blackjack - dealer wins");
                ret = WinLoss.Dealer;
            } else if (DealerHand.IsBust || on.IsBlackjack) {
                l("Dealer busted or Hand is blackjack - player wins");
                ret = WinLoss.Player;
            } else {
                l(String.Format("No special case. Hand Sum: {0} Dealer Sum: {1}", on.Sum, DealerHand.Sum));
                if (on.Sum > DealerHand.Sum) {
                    l("Player wins");
                    ret = WinLoss.Player;
                } else if (on.Sum < DealerHand.Sum) {
                    l("Dealer wins");
                    ret = WinLoss.Dealer;
                } else {
                    l("Push");
                    ret = WinLoss.Push;
                }
            }

            l("Updating statistics");
            switch (ret) {
                case WinLoss.Dealer:
                    _numLosses++;
                    _thisLoss += on.Bet;
                    break;
                case WinLoss.Player:
                    _numWins++;
                    _thisWin += on.Bet;
                    break;
                default:
                    break;
            }
            l(String.Format("Wins:Losses;Win:Loss::{0}:{1};{2}:{3}", NumWins, NumLosses, LargestWin, LargestLoss));

            if (ret != WinLoss.NoWin) {
                l("Applying bet");
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
        public string PlayerFunds {
            get {
                return "Funds: $" + (_playerHand != null ? _playerHand.Cash : 500);
            }
        }
        public string PlayerNormalBet {
            get {
                return "$" + _playerHand.Bet.ToString();
            }
        }
        public string PlayerSplitBet {
            get {
                return _playerHand.HasSplit ? "$" + _playerHand.SplitHand.Bet.ToString() : "";
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
        public void NotifyAll() {
            foreach (string prop in all) {
                Notify(prop);
            }
        }
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _displayHole;
        private uint _thisLoss;
        private uint _thisWin;

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

        internal void NewRound(UI_Sketch ui = null) {
            l("New Round", false);
            _playerHand.PutCardsBack();

            _dealerHand.DiscardAll();
            NotifyAll();
            _displayHole = false;
            ActiveHand = ActiveHandPotentials.Normal;

            if (PlayerHand.Cash < MIN_BET) {
                l("Cash is less than MIN_BET", false);
                throw new IllegalMoveException("I can't do that, Dave.");
            }
            l("Retrieving bet", false);
            var b = new GetBet(this, ui);
            l("Making bet", false);
            PlayerHand.makeBet(b.Bet);
            b.Close();

            l("Player draws 2 cards", false);
            _playerHand.Draw(2);

            l("Dealer draws 2 cards", false);
            _dealerHand.Draw(2);
            NotifyAll();
            l("Cards drawn");

            if (_playerHand.IsBlackjack) {
                l("Player drew Blackjack");
                throw new BlackjackException();
            }

            NotifyAll();
        }

        public WinLoss?[] EndRound() {
            l("Ending round", false);
            ActiveHand = ActiveHandPotentials.None;
            l("Getting result of normal hand", false);
            WinLoss Hand1 = Winner(PlayerHand);
            WinLoss? Hand2 = null;
            if (PlayerHand.HasSplit) {
                l("Getting result of stood hand", false);
                Hand2 = Winner(PlayerHand.SplitHand);
                l("Applying split hand's results to normal hand", false);
                PlayerHand.Cash += PlayerHand.SplitHand.Cash;
            }
            WinLoss?[] ret = { Hand1, Hand2 };

            l("Updating statistics", false);
            _largestWin = _thisWin > _largestWin ? _thisWin : _largestWin;
            _largestLoss = _thisLoss > _largestLoss ? _thisLoss : _largestLoss;
            _thisWin = _thisLoss = 0;
            l(String.Format("Wins:Losses;Win:Loss::{0}:{1};{2}:{3}", NumWins, NumLosses, LargestWin, LargestLoss));
            l("Round ended");
            return ret;
        }

        public void WriteLogToFile(string path) {
            bool log_mode = false;
            if (log_mode) {
                if (System.IO.File.Exists(path)) {
                    System.IO.File.AppendAllLines(path, log);
                } else {
                    System.IO.File.WriteAllLines(path, log);
                }
                log.Clear();
            }
        }
    }
}
