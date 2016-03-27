using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;
using HrtzCraft.Extensions;
using HrtzCraft.Relays;
using HrtzCraft.Utilities;

namespace HrtzCraft.ViewModels
{
    public class ServerControlViewModel : ObservableObject
    {
        #region Constructor

        #endregion

        #region Private Fields

        private StreamWriter _inputWriter;
        private string _consoleOutput;
        private ICommand _startServerCommand;
        private ICommand _sendCommand;

        #endregion

        #region Public Fields

        public static ServerControlViewModel Instance { get; } = new ServerControlViewModel();

        #endregion

        #region Properties

        public string ConsoleOutput
        {
            get { return _consoleOutput; }
            set
            {
                if (value == _consoleOutput) return;
                _consoleOutput = value;
                OnPropertyChanged();
            }
        }

        private bool CanExecute_StartServer()
        {
            return Directory.Exists(ServerConfigViewModel.Instance.Server.ServerDirectory) && File.Exists(ServerConfigViewModel.Instance.Server.JavaFullName);
        }

        #endregion

        #region Commands

        public ICommand StartServerCommand
        {
            get
            {
                return _startServerCommand ??
                       (_startServerCommand = new RelayCommand(Execute_StartServer, p => CanExecute_StartServer()));
            }
        }

        public ICommand SendCommand
        {
            get
            {
                return _sendCommand ??
                       (_sendCommand = new RelayCommand(p => Execute_Send(p as string), p => p is string && ServerConfigViewModel.Instance.Server.IsRunning));
            }
        }

        #endregion

        #region Methods

        private async void Execute_StartServer(object obj)
        {
            if (ServerConfigViewModel.Instance.Server.IsRunning)
            {
                _inputWriter.WriteLine("stop");
                return;
            }

            using (var process = new Process())
            {
                process.StartInfo.FileName = ServerConfigViewModel.Instance.Server.JavaFullName;
                process.StartInfo.WorkingDirectory = ServerConfigViewModel.Instance.Server.ServerDirectory;
                process.StartInfo.Arguments = $"-Xms{ServerConfigViewModel.Instance.Server.RamAllocated}M -Xmx{ServerConfigViewModel.Instance.Server.RamMax}M -jar {ServerConfigViewModel.Instance.Server.JarFilename}";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;

                process.ErrorDataReceived += (sender, args) =>
                {
                    if (args.Data == null) return;
                    ProcessConsoleOutput(args.Data);
                };

                process.OutputDataReceived += (sender, args) =>
                {
                    if (args.Data == null) return;
                    ProcessConsoleOutput(args.Data);
                };

                process.Exited += (sender, args) =>
                {
                    ServerConfigViewModel.Instance.Server.IsRunning = false;
                };

                Exception exception = null;

                try
                {
                    process.Start();
                }
                catch (Exception ex)
                {
                    exception = ex;
                    Logger.Write("Could not start the server", true, exception);
                }
                finally
                {
                    if (exception == null)
                        ServerConfigViewModel.Instance.Server.IsRunning = true;
                }

                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

                _inputWriter = process.StandardInput;
                _inputWriter.AutoFlush = true;

                await process.WaitForExitAsync();
            }
        }

        private void ProcessConsoleOutput(string data)
        {
            // Eula
            if (data.Contains("You need to agree to the EULA in order to run the server."))
            {
                if (!File.Exists(Path.Combine(ServerConfigViewModel.Instance.Server.ServerDirectory, "eula.txt"))) return;

                ConsoleOutput = "The server has automatically been shut down, and the file 'eula.txt' has been opened. Set eula to true in the opened file, and restart!";
                Process.Start(Path.Combine(ServerConfigViewModel.Instance.Server.ServerDirectory, "eula.txt"));
            }

            // Logged in
            if (data.Contains(" logged in with entity id "))
            {
                GlobalPlayersViewModel.Instance.PlayerJoinedEvent(data);

                if (SendCommand.CanExecute("list"))
                    SendCommand.Execute("list");
            }

            // Logged out
            if (data.Contains(" lost connection:"))
            {
                GlobalPlayersViewModel.Instance.PlayerLeaveEvent(data);

                if (SendCommand.CanExecute("list"))
                    SendCommand.Execute("list");
            }

            // Op
            if (data.Contains(": Opped ") || data.Contains(": De-opped "))
                GlobalPlayersViewModel.Instance.PlayerOpEvent(data);

            // Ban
            if (data.Contains(": Banned player ") || data.Contains(": Unbanned player "))
                GlobalPlayersViewModel.Instance.PlayerBanEvent(data);

            // Whitelist
            if (data.Contains(" to the whitelist") || data.Contains(" from the whitelist"))
                GlobalPlayersViewModel.Instance.PlayerWhitelistEvent(data);

            // List
            if (Regex.IsMatch(data, @"There are (\d+)\/(\d+) players online:"))
                GlobalPlayersViewModel.Instance.PlayerListEvent(data);

            // Done -> list
            if (Regex.IsMatch(data, @"Done \((.*)s\)! For help, type"))
                if (SendCommand.CanExecute("list"))
                    SendCommand.Execute("list");

            // Finally, send the output to console window
            ConsoleOutput += (data + Environment.NewLine);
        }

        private void Execute_Send(string value)
        {
            Logger.Write($"Executing command: {value}");
            _inputWriter?.WriteLine(value);
        }

        #endregion
    }
}
