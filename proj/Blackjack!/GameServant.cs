using System;
using System.ComponentModel;

namespace Blackjack
{
    public class GameServant : INotifyPropertyChanged
    {
        private CardCollection _source;
        private CardCollection _discard;
        private PlayerHand _playerHand;
        private DealerHand _dealerHand;

        public enum ActiveHandPotentials
        {
            Normal,
            Split
        }

        private string _playerName;
        

        public string PlayerName
        {
            get { return _playerName; }
            set
            {
                _playerName = value;
                Notify("PlayerName");
            }
        }
        public uint PlayerFunds {
            get { return _playerHand.Cash; }
        }

        public GameServant()
        {
            this.ActiveHand = ActiveHandPotentials.Split;
            this._source = new CardCollection(1);
            this._discard = new CardCollection();
            this._playerHand = new PlayerHand(_source, _discard);
            this._dealerHand = new DealerHand(_source, _discard);
        }

        private ActiveHandPotentials _activeHand;
        public ActiveHandPotentials ActiveHand
        {
            get { return _activeHand; }
            set
            {
                _activeHand = value;
                Notify("ActiveHand");
                Notify("IsNormalActive");
                Notify("IsSplitActive");
            }
        }
        public bool IsNormalActive { get { return ActiveHand == ActiveHandPotentials.Normal; } }
        public bool IsSplitActive  { get { return ActiveHand == ActiveHandPotentials.Split;  } }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property. // The CallerMemberName attribute that is applied to the optional propertyName // parameter causes the property name of the caller to be substituted as an argument.
        void Notify(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
