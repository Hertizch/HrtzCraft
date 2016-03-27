using System.Diagnostics;
using HrtzCraft.Extensions;

namespace HrtzCraft.Models
{
    public class Server : ObservableObject
    {
        private string _displayName;
        private string _serverDirectory;
        private bool _isRunning;
        private string _jarFilename;
        private string _javaFullName;
        private int _ramAllocated;
        private int _ramMaximum;
        private int _playersOnline;
        private int _maxPlayers;

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (value == _displayName) return;
                _displayName = value;
                OnPropertyChanged();
            }
        }

        public string ServerDirectory
        {
            get { return _serverDirectory; }
            set
            {
                if (value == _serverDirectory) return;
                _serverDirectory = value;
                OnPropertyChanged();
            }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                if (value == _isRunning) return;
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        public string JarFilename
        {
            get { return _jarFilename; }
            set
            {
                if (value == _jarFilename) return;
                _jarFilename = value;
                OnPropertyChanged();
            }
        }

        public string JavaFullName
        {
            get { return _javaFullName; }
            set
            {
                if (value == _javaFullName) return;
                _javaFullName = value;
                OnPropertyChanged();
            }
        }

        public int RamAllocated
        {
            get { return _ramAllocated; }
            set
            {
                if (value == _ramAllocated) return;
                _ramAllocated = value;
                OnPropertyChanged();
            }
        }

        public int RamMax
        {
            get { return _ramMaximum; }
            set
            {
                if (value == _ramMaximum) return;
                _ramMaximum = value;
                OnPropertyChanged();
            }
        }

        public int PlayersOnline
        {
            get { return _playersOnline; }
            set
            {
                if (value == _playersOnline) return;
                _playersOnline = value;
                OnPropertyChanged();
            }
        }

        public int MaxPlayers
        {
            get { return _maxPlayers; }
            set
            {
                if (value == _maxPlayers) return;
                _maxPlayers = value;
                OnPropertyChanged();
            }
        }
    }
}
