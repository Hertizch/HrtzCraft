using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using HrtzCraft.Extensions;
using HrtzCraft.Models;
using HrtzCraft.ViewModels;

namespace HrtzCraft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string _searchFilter;

        public string SearchFilter
        {
            get { return _searchFilter; }
            set
            {
                if (value == _searchFilter) return;
                _searchFilter = value;
                OnPropertyChanged();

                if (!string.IsNullOrEmpty(_searchFilter))
                    AddFilter();

                ((CollectionViewSource)Resources["GlobalPlayersViewSource"])?.View.Refresh();
            }
        }

        void PlayerList_Filter(object sender, FilterEventArgs e)
        {
            var player = e.Item as GlobalPlayer;

            if (player == null)
                e.Accepted = false;
            else if (!player.Name.Contains(_searchFilter, StringComparison.OrdinalIgnoreCase))
                e.Accepted = false;
        }

        private void AddFilter()
        {
            ((CollectionViewSource)Resources["GlobalPlayersViewSource"]).Filter -= PlayerList_Filter;
            ((CollectionViewSource)Resources["GlobalPlayersViewSource"]).Filter += PlayerList_Filter;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (ServerConfigViewModel.Instance.Server.IsRunning)
            {
                MessageBox.Show("The server is currently online. Shut the server down before exiting.");
                e.Cancel = true;
                return;
            }

            if (ServerConfigViewModel.Instance.SaveServerConfigToFileCommand.CanExecute(ServerConfigViewModel.Instance.ServerSettingsFullPath))
                ServerConfigViewModel.Instance.SaveServerConfigToFileCommand.Execute(ServerConfigViewModel.Instance.ServerSettingsFullPath);

            if (BuildToolsViewModel.Instance.SaveBuildToolsConfigToFileCommand.CanExecute(BuildToolsViewModel.Instance.BuildToolsSettingsFullPath))
                BuildToolsViewModel.Instance.SaveBuildToolsConfigToFileCommand.Execute(BuildToolsViewModel.Instance.BuildToolsSettingsFullPath);

            Properties.Settings.Default.Save();
        }
    }
}
