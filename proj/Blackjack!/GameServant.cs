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
            Split
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

        private PlayerHand _playerHand; private ActiveHandPotentials _activeHand;
        private DealerHand _dealerHand;
        #endregion
        #endregion

        public GameServant()
        {
            this.ActiveHand = ActiveHandPotentials.Normal;
            this._source = new CardCollection(1);
            this._discard = new CardCollection();
            this._playerHand = new PlayerHand(_source, _discard);
            this._dealerHand = new DealerHand(_source, _discard);
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
        public uint PlayerFunds { get { return _playerHand.Cash; } }
        public List<Card> PlayerNormalCards { get { return _playerHand.Cards; } }
        public List<Card> PlayerSplitCards { get { return _playerHand.SplitHand.Cards; } }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property. // The CallerMemberName attribute that is applied to the optional propertyName // parameter causes the property name of the caller to be substituted as an argument.
        void Notify(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
        #endregion
    }
}
