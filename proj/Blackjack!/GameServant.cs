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



        #region Instance Variables
        #region Front-matter
        private string _playerName;
        private uint _numWins;

        public uint NumWins {
            get {
                return _numWins;
            }
            set {
                _numWins = value;
            }
        }
        private uint _numLosses;

        public uint NumLosses {
            get {
                return _numLosses;
            }
            set {
                _numLosses = value;
            }
        }
        private uint _largestWin;

        public uint LargestWin {
            get {
                return _largestWin;
            }
            set {
                _largestWin = value;
            }
        }
        private uint _largestLoss;

        public uint LargestLoss {
            get {
                return _largestLoss;
            }
            set {
                _largestLoss = value;
            }
        }
        #endregion

        #region Back-matter
        public static uint NUM_DECKS = 1;
        public static uint MIN_BET = 1;
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
        }

        public void init() {
            //isinit = true;
            this.ActiveHand = ActiveHandPotentials.Normal;
            this._playerHand = new PlayerHand(_source, _discard);
            this._dealerHand = new DealerHand(_source, _discard);
        }

        public void Hit() {
            switch (ActiveHand) {
                case ActiveHandPotentials.Normal:
                    PlayerHand.Draw();
                    Notify("PlayerNormalCards");
                    break;
                case ActiveHandPotentials.Split:
                    PlayerHand.SplitHand.Draw();
                    Notify("PlayerSplitCards");
                    break;
                case ActiveHandPotentials.None:
                default:
                    throw new Exception("Oh no!");
            }
        }

        public void Split() {
            switch (ActiveHand) {
                case ActiveHandPotentials.Normal:
                    PlayerHand.Split();
                    Notify("PlayerNormalCards");
                    Notify("PlayerSplitCards");
                    break;
                case ActiveHandPotentials.Split:
                case ActiveHandPotentials.None:
                default:
                    throw new Exception("Oh no!");
            }
        }

        public void Stand() {
            while (DealerHand.Sum <= 17) {
                DealerHand.Draw();
            }
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
        /// <summary>
        /// Compares 'on' to the dealer's hand for win/loss
        /// </summary>
        /// <param name="on">a playerhand to compare against</param>
        /// <returns>Who won the round, if either.</returns>
        public WinLoss Winner(PlayerHand on) {
            /*if (on.IsBust && DealerHand.IsBust) {
                if (on.Sum < DealerHand.Sum) {
                    return WinLoss.Player;
                } else if (on.Sum > DealerHand.Sum) {
                    return WinLoss.Dealer;
                } else {
                    return WinLoss.Push;
                }
            }

            if (on.IsBlackjack && DealerHand.IsBlackjack) {
                return WinLoss.Push;
            }

            if (on.IsBust) {
                return WinLoss.Dealer;
            }

            if (DealerHand.IsBust) {
                return WinLoss.Dealer;
            }

            if (on.Sum > DealerHand.Sum) {
                return WinLoss.Player;
            } else if (on.Sum < DealerHand.Sum) {
                return WinLoss.Dealer;
            } else {
                return WinLoss.NoWin;
            }*/

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

            return ret;
        }
        
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        public const int MINBET = 20;

        // This method is called by the Set accessor of each property.
        // parameter causes the property name of the caller to be substituted as an argument.
        void Notify(string propName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
        #endregion

        internal void GetBetAndDeal() {
            if (PlayerHand.Cash < 20) {
                throw new InvalidOperationException("I can't do that, Dave.");
            }
            var b = new GetBet(this);
            b.ShowDialog();
            PlayerHand.makeBet(b.Bet);
            b.Close();
            _playerHand.Draw(2);
            _dealerHand.Draw(2);
            Notify("PlayerFunds");
            Notify("PlayerNormalBet");
        }

        internal void NewRound() {
            if (_playerHand.HasSplit) {
                _playerHand.doBet(Winner(_playerHand.SplitHand));
            }
            _playerHand.doBet(Winner(_playerHand));
            _playerHand.DiscardAll();
            _dealerHand.DiscardAll();
            ActiveHand = ActiveHandPotentials.Normal;
            GetBetAndDeal();
        }
    }
}
