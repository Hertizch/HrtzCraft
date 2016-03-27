using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using HrtzCraft.Extensions;
using HrtzCraft.Relays;

namespace HrtzCraft.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        public ShellViewModel()
        {
            
        }

        private ICommand _closeApplicationCommand;
        private ICommand _openAppMenuCommand;

        public ICommand CloseApplicationCommand
        {
            get
            {
                return _closeApplicationCommand ??
                       (_closeApplicationCommand = new RelayCommand(Execute_CloseApplication, p => true));
            }
        }

        private static void Execute_CloseApplication(object obj)
        {
            if (ServerConfigViewModel.Instance.Server.IsRunning)
            {
                MessageBox.Show("The server is currently online. Shut the server down before exiting.");
                return;
            }

            if (ServerConfigViewModel.Instance.SaveServerConfigToFileCommand.CanExecute(ServerConfigViewModel.Instance.ServerSettingsFullPath))
                ServerConfigViewModel.Instance.SaveServerConfigToFileCommand.Execute(ServerConfigViewModel.Instance.ServerSettingsFullPath);

            if (BuildToolsViewModel.Instance.SaveBuildToolsConfigToFileCommand.CanExecute(BuildToolsViewModel.Instance.BuildToolsSettingsFullPath))
                BuildToolsViewModel.Instance.SaveBuildToolsConfigToFileCommand.Execute(BuildToolsViewModel.Instance.BuildToolsSettingsFullPath);

            Properties.Settings.Default.Save();

            Application.Current.MainWindow.Close();
        }

        public ICommand OpenAppMenuCommand
        {
            get
            {
                return _openAppMenuCommand ??
                       (_openAppMenuCommand =
                           new RelayCommand(p => Execute_OpenAppMenu(p as ToggleButton), p => p is ToggleButton));
            }
        }

        private static void Execute_OpenAppMenu(FrameworkElement toggleButton)
        {
            if (toggleButton == null) return;

            toggleButton.ContextMenu.IsEnabled = true;
            toggleButton.ContextMenu.PlacementTarget = toggleButton;
            toggleButton.ContextMenu.Placement = PlacementMode.Bottom;
            toggleButton.ContextMenu.IsOpen = true;
        }
    }
}
