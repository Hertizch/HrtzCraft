using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using HrtzCraft.Extensions;
using HrtzCraft.Interfaces;
using HrtzCraft.Models;
using HrtzCraft.Properties;
using HrtzCraft.Relays;

namespace HrtzCraft.ViewModels
{
    public class ServerConfigViewModel : ObservableObject, IPageViewModel
    {
        #region Constructor

        public ServerConfigViewModel()
        {
            if (LoadServerConfigFromFileCommand.CanExecute(ServerSettingsFullPath))
                LoadServerConfigFromFileCommand.Execute(ServerSettingsFullPath);
        }

        #endregion

        #region Private Fields

        private ICommand _saveServerConfigToFileCommand;
        private ICommand _loadServerConfigFromFileCommand;
        private ICommand _resetUiPropertiesCommand;
        private ICommand _browseServerDirectoryCommand;
        private ICommand _browseServerJarFileCommand;
        private ICommand _browseJavaExeCommand;

        #endregion

        #region Public Fields

        public string Name => "Configuration";
        public static ServerConfigViewModel Instance { get; } = new ServerConfigViewModel();
        public Server Server { get; set; } = new Server();
        public string ServerSettingsFullPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "servers.xml");

        #endregion

        #region Properties
        #endregion

        #region Commands

        public ICommand SaveServerConfigToFileCommand
        {
            get
            {
                return _saveServerConfigToFileCommand ??
                       (_saveServerConfigToFileCommand = new RelayCommand(p => Execute_SaveServerConfigToFile(p as string), p => p is string));
            }
        }

        public ICommand LoadServerConfigFromFileCommand
        {
            get
            {
                return _loadServerConfigFromFileCommand ??
                       (_loadServerConfigFromFileCommand = new RelayCommand(p => Execute_LoadServerConfigFromFile(p as string), p => p is string && File.Exists(p.ToString())));
            }
        }

        public ICommand ResetUiPropertiesCommand
        {
            get
            {
                return _resetUiPropertiesCommand ??
                       (_resetUiPropertiesCommand = new RelayCommand(Execute_ResetUiProperties, p => true));
            }
        }

        public ICommand BrowseServerDirectoryCommand
        {
            get
            {
                return _browseServerDirectoryCommand ??
                       (_browseServerDirectoryCommand = new RelayCommand(Execute_BrowseServerDirectory, p => true));
            }
        }

        public ICommand BrowseServerJarFileCommand
        {
            get
            {
                return _browseServerJarFileCommand ??
                       (_browseServerJarFileCommand = new RelayCommand(Execute_BrowseServerJarFile, p => true));
            }
        }

        public ICommand BrowseJavaExeCommand
        {
            get
            {
                return _browseJavaExeCommand ??
                       (_browseJavaExeCommand = new RelayCommand(Execute_BrowseJavaExe, p => true));
            }
        }

        #endregion

        #region Methods

        private void Execute_SaveServerConfigToFile(string filename)
        {
            Serializer.SerializeToXml(Server, filename);
        }

        private void Execute_LoadServerConfigFromFile(string filename)
        {
            Server = Serializer.DeserializeFromXml<Server>(filename);
        }

        private static void Execute_ResetUiProperties(object obj)
        {
            Settings.Default.Reset();
        }

        private void Execute_BrowseServerDirectory(object obj)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = @"Select the folder where your server is located.";
                dialog.SelectedPath = Server.ServerDirectory ?? Environment.SpecialFolder.MyComputer.ToString();
                dialog.ShowNewFolderButton = true;

                var result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                    Server.ServerDirectory = dialog.SelectedPath;
            }
        }

        private void Execute_BrowseServerJarFile(object obj)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = @"Select your Server .jar file.";
                dialog.InitialDirectory = Server.ServerDirectory ?? Environment.SpecialFolder.MyComputer.ToString();
                dialog.CheckFileExists = true;
                dialog.FileName = "*.jar";

                var result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                    Server.JarFilename = Path.GetFileName(dialog.FileName);
            }
        }

        private void Execute_BrowseJavaExe(object obj)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = @"Select the Java executable.";
                dialog.InitialDirectory = Environment.GetEnvironmentVariable("PATH") ?? Environment.SpecialFolder.ProgramFiles.ToString();
                dialog.CheckFileExists = true;
                dialog.FileName = "java.exe";

                var result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                    Server.JavaFullName = dialog.FileName;
            }
        }

        #endregion
    }
}
