using HrtzCraft.Extensions;

namespace HrtzCraft.Models
{
    public class GlobalPlayer : ObservableObject
    {
        #region Private Fields

        private string _name;
        private string _uuid;
        private string _opLevel;
        private bool _isOperator;
        private bool _isWhitelisted;
        private bool _isBanned;
        private bool _isOnline;
        private string _banReason;
        private string _banSource;
        private string _banExpires;
        private string _groupName;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the players name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the players unique UUID
        /// </summary>
        public string Uuid
        {
            get { return _uuid; }
            set
            {
                if (value == _uuid) return;
                _uuid = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the players OP level
        /// </summary>
        public string OpLevel
        {
            get { return _opLevel; }
            set
            {
                if (value == _opLevel) return;
                _opLevel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets if the player has OP rights
        /// </summary>
        public bool IsOperator
        {
            get { return _isOperator; }
            set
            {
                if (value == _isOperator) return;
                _isOperator = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets if the player has is whitelisted
        /// </summary>
        public bool IsWhitelisted
        {
            get { return _isWhitelisted; }
            set
            {
                if (value == _isWhitelisted) return;
                _isWhitelisted = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets if the player is banned
        /// </summary>
        public bool IsBanned
        {
            get { return _isBanned; }
            set
            {
                if (value == _isBanned) return;
                _isBanned = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets if the player is currently online
        /// </summary>
        public bool IsOnline
        {
            get { return _isOnline; }
            set
            {
                if (value == _isOnline) return;
                _isOnline = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets if the reason for being banned
        /// </summary>
        public string BanReason
        {
            get { return _banReason; }
            set
            {
                if (value == _banReason) return;
                _banReason = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets who issued the ban
        /// </summary>
        public string BanSource
        {
            get { return _banSource; }
            set
            {
                if (value == _banSource) return;
                _banSource = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets when the ban exires
        /// </summary>
        public string BanExpires
        {
            get { return _banExpires; }
            set
            {
                if (value == _banExpires) return;
                _banExpires = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets when the ban exires
        /// </summary>
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                if (value == _groupName) return;
                _groupName = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
