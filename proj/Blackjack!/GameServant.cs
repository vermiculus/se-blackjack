using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Blackjack
{
    public class GameServant : INotifyPropertyChanged
    {
        public enum ActiveHandPotentials
        {
            Normal,
            Split,
            None
        }

        #region Instance Variables
        #region Front-matter
        private string _playerName;
        private uint _numWins;
        private uint _numLosses;
        private uint _largestWin;
        private uint _largestLoss;
        #endregion

        #region Back-matter
        public static uint NUM_DECKS = 1;
        public static uint MIN_BET = 1;
        private CardCollection _source;
        private CardCollection _discard;

        private PlayerHand _playerHand;

        internal PlayerHand PlayerHand
        {
            get { return _playerHand; }
        }
        private ActiveHandPotentials _activeHand;
        private DealerHand _dealerHand;
        internal DealerHand DealerHand
        {
            get { return _dealerHand; }
        }
        #endregion
        #endregion

        public GameServant()
        {
            this.ActiveHand = ActiveHandPotentials.None;
            this._source = new CardCollection(1);
            this._discard = new CardCollection();
        }

        public void init()
        {
            isinit = true;
            this.ActiveHand = ActiveHandPotentials.Normal;
            this._playerHand = new PlayerHand(_source, _discard);
            this._dealerHand = new DealerHand(_source, _discard);
        }

        public void Hit()
        {
            switch (ActiveHand)
            {
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

        public void Stand()
        {
            switch (ActiveHand)
            {
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

        #region Properties and Events
        public ActiveHandPotentials ActiveHand
        {   get { return _activeHand; }
            set
            {
                _activeHand = value;
                Notify("ActiveHand");     // The active hand has changed,
                Notify("IsNormalActive"); // and thus so have these
                Notify("IsSplitActive");  // two properties (used to control which button group is enabled)
        }   }
        
        public bool IsNormalActive { get { return ActiveHand == ActiveHandPotentials.Normal; } }
        public bool IsSplitActive  { get { return ActiveHand == ActiveHandPotentials.Split;  } }
        public string PlayerName { get { return _playerName; } set { _playerName = value; Notify("PlayerName"); } }
        public uint PlayerFunds { get { return _playerHand != null ? _playerHand.Cash : 500; } }
        public string PlayerNormalBet { get { return _playerHand.Bet.ToString(); } }
        public string PlayerSplitBet { get { return _playerHand.HasSplit ? _playerHand.SplitHand.Bet.ToString() : ""; } }
        public bool CanPlayerSplit { get { return PlayerHand.CanSplit; } }
        public List<Card> PlayerNormalCards { get { return _playerHand.Cards; } }
        public List<Card> PlayerSplitCards { get { return _playerHand.SplitHand.Cards; } }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isinit;
        public const int MINBET = 20;

        // This method is called by the Set accessor of each property.
        // parameter causes the property name of the caller to be substituted as an argument.
        void Notify(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
        #endregion
    }
}
