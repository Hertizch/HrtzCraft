using System.Deployment.Application;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using HrtzCraft.Extensions;
using HrtzCraft.Relays;

namespace HrtzCraft.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        private ICommand _closeApplicationCommand;
        private ICommand _openAppMenuCommand;

        public string CurrentVersion => ApplicationDeployment.IsNetworkDeployed ? "HrtzCraft - " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
            : "HrtzCraft - " + Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + "." + Assembly.GetExecutingAssembly().GetName().Version.Build;

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
