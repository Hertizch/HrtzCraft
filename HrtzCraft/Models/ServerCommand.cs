using HrtzCraft.Enums;
using HrtzCraft.Extensions;

namespace HrtzCraft.Models
{
    public class ServerCommand : ObservableObject
    {
        private string _displayName;
        private string _command;
        private string _bukkitPermission;
        private string _mojangPermission;
        private ServerCommandTarget _serverCommandTarget;
        private ServerCommandPermissionDefault _serverCommandPermissionDefault;

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

        public string Command
        {
            get { return _command; }
            set
            {
                if (value == _command) return;
                _command = value;
                OnPropertyChanged();
            }
        }

        public string BukkitPermission
        {
            get { return _bukkitPermission; }
            set
            {
                if (value == _bukkitPermission) return;
                _bukkitPermission = value;
                OnPropertyChanged();
            }
        }

        public string MojangPermission
        {
            get { return _mojangPermission; }
            set
            {
                if (value == _mojangPermission) return;
                _mojangPermission = value;
                OnPropertyChanged();
            }
        }

        public ServerCommandTarget ServerCommandTarget
        {
            get { return _serverCommandTarget; }
            set
            {
                if (value == _serverCommandTarget) return;
                _serverCommandTarget = value;
                OnPropertyChanged();
            }
        }

        public ServerCommandPermissionDefault ServerCommandPermissionDefault
        {
            get { return _serverCommandPermissionDefault; }
            set
            {
                if (value == _serverCommandPermissionDefault) return;
                _serverCommandPermissionDefault = value;
                OnPropertyChanged();
            }
        }
    }
}
