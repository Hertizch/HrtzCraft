using System.Diagnostics;
using HrtzCraft.Extensions;

namespace HrtzCraft.Models
{
    public class BuildTool : ObservableObject
    {
        private string _buildToolsDirectory;
        private string _buildToolsUri;
        private string _buildToolsRssUri;
        private string _gitBashFullName;
        private string _spigotVersion;

        public string BuildToolsDirectory
        {
            get { return _buildToolsDirectory; }
            set
            {
                if (value == _buildToolsDirectory) return;
                _buildToolsDirectory = value;
                OnPropertyChanged();
            }
        }

        public string BuildToolsUri
        {
            get { return _buildToolsUri; }
            set
            {
                if (value == _buildToolsUri) return;
                _buildToolsUri = value;
                OnPropertyChanged();
            }
        }

        public string BuildToolsRssUri
        {
            get { return _buildToolsRssUri; }
            set
            {
                if (value == _buildToolsRssUri) return;
                _buildToolsRssUri = value;
                OnPropertyChanged();
            }
        }

        public string GitBashFullName
        {
            get { return _gitBashFullName; }
            set
            {
                if (value == _gitBashFullName) return;
                _gitBashFullName = value;
                OnPropertyChanged();
            }
        }

        public string SpigotVersion
        {
            get { return _spigotVersion; }
            set
            {
                if (value == _spigotVersion) return;
                _spigotVersion = value;
                OnPropertyChanged();
            }
        }
    }
}
