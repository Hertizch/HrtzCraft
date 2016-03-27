using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using HrtzCraft.Extensions;
using HrtzCraft.Models;
using HrtzCraft.Models.PlayerDataFiles;
using HrtzCraft.Relays;
using HrtzCraft.Utilities;
using Newtonsoft.Json;

namespace HrtzCraft.ViewModels
{
    public class GlobalPlayersViewModel : ObservableObject
    {
        #region Constructor

        public GlobalPlayersViewModel()
        {
            if (LoadPlayerDataFromFileCommand.CanExecute("usercache.json"))
                LoadPlayerDataFromFileCommand.Execute("usercache.json");

            if (LoadPlayerDataFromFileCommand.CanExecute("ops.json"))
                LoadPlayerDataFromFileCommand.Execute("ops.json");

            if (LoadPlayerDataFromFileCommand.CanExecute("whitelist.json"))
                LoadPlayerDataFromFileCommand.Execute("whitelist.json");

            if (LoadPlayerDataFromFileCommand.CanExecute("banned-players.json"))
                LoadPlayerDataFromFileCommand.Execute("banned-players.json");
        }

        #endregion

        #region Private Fields

        private GlobalPlayer _selectedPlayer;
        private ICommand _loadPlayerDataFromFileCommand;

        #endregion

        #region Public Fields

        public static GlobalPlayersViewModel Instance { get; } = new GlobalPlayersViewModel();
        public ObservableList<GlobalPlayer> GlobalPlayersCollection { get; set; } = new ObservableList<GlobalPlayer>();

        #endregion

        #region Properties

        public GlobalPlayer SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                if (value == _selectedPlayer) return;
                _selectedPlayer = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand LoadPlayerDataFromFileCommand
        {
            get
            {
                return _loadPlayerDataFromFileCommand ??
                       (_loadPlayerDataFromFileCommand =
                           new RelayCommand(p => Execute_LoadPlayerDataFromFile(p as string), p => p is string));
            }
        }

        #endregion

        #region Methods

        private void Execute_LoadPlayerDataFromFile(string filename)
        {
            if (!Directory.Exists(ServerConfigViewModel.Instance.Server.ServerDirectory))
            {
                Logger.Write($"Could not get player info from '{filename}'. The server directory ('{ServerConfigViewModel.Instance.Server.ServerDirectory}') is empty or does not exist.", true);
                return;
            }

            FileStream stream = null;

            try
            {
                stream = new FileStream(Path.Combine(ServerConfigViewModel.Instance.Server.ServerDirectory, filename), FileMode.Open,
                    FileAccess.Read, FileShare.ReadWrite);

                using (var streamReader = new StreamReader(stream))
                {
                    stream = null;

                    var json = streamReader.ReadToEnd();

                    switch (filename)
                    {
                        case "usercache.json":
                        {
                            var userCacheCollection = JsonConvert.DeserializeObject<List<UserCache>>(json);

                            if (userCacheCollection != null)
                            {
                                foreach (var user in from user in userCacheCollection
                                    let userExists = GlobalPlayersCollection.Any(x => x.Uuid == user.Uuid)
                                    where !userExists
                                    select user)
                                {
                                    GlobalPlayersCollection.Add(new GlobalPlayer
                                    {
                                        Name = user.Name,
                                        Uuid = user.Uuid
                                    });
                                }

                                userCacheCollection.Clear();
                            }

                            break;
                        }

                        case "ops.json":
                        {
                            var opsCollection = JsonConvert.DeserializeObject<List<Op>>(json);

                            if (opsCollection != null)
                            {
                                // Remove existing true values
                                foreach (var source in GlobalPlayersCollection.Where(x => x.IsOperator))
                                {
                                    source.IsOperator = false;
                                    source.OpLevel = null;
                                }

                                foreach (var globalPlayer in GlobalPlayersCollection)
                                {
                                    foreach (var op in opsCollection.Where(x => globalPlayer.Uuid.Equals(x.Uuid)))
                                    {
                                        globalPlayer.IsOperator = true;
                                        globalPlayer.OpLevel = op.Level;
                                    }
                                }

                                opsCollection.Clear();
                            }

                            break;
                        }

                        case "banned-players.json":
                        {
                            var bannedPlayersCollection = JsonConvert.DeserializeObject<List<BannedPlayer>>(json);

                            if (bannedPlayersCollection != null)
                            {
                                // Remove existing true values
                                foreach (var source in GlobalPlayersCollection.Where(x => x.IsBanned))
                                {
                                    source.IsBanned = false;
                                    source.BanExpires = null;
                                    source.BanReason = null;
                                    source.BanSource = null;
                                }

                                foreach (var globalPlayer in GlobalPlayersCollection)
                                {
                                    foreach (
                                        var bannedPlayer in
                                            bannedPlayersCollection.Where(x => globalPlayer.Uuid.Equals(x.Uuid)))
                                    {
                                        globalPlayer.IsBanned = true;
                                        globalPlayer.BanReason = bannedPlayer.Reason;
                                        globalPlayer.BanSource = bannedPlayer.Source;
                                        globalPlayer.BanExpires = bannedPlayer.Expires;
                                    }
                                }

                                bannedPlayersCollection.Clear();
                            }

                            break;
                        }

                        case "whitelist.json":
                        {
                            var whitelistedCollection = JsonConvert.DeserializeObject<List<Whitelist>>(json);

                            if (whitelistedCollection != null)
                            {
                                // Remove existing true values
                                foreach (var source in GlobalPlayersCollection.Where(x => x.IsWhitelisted))
                                    source.IsWhitelisted = false;

                                foreach (var globalPlayer in from globalPlayer in GlobalPlayersCollection
                                    from whitelist in whitelistedCollection.Where(x => globalPlayer.Uuid.Equals(x.Uuid))
                                    select globalPlayer)
                                    globalPlayer.IsWhitelisted = true;

                                whitelistedCollection.Clear();
                            }

                            break;
                        }
                    }
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        private bool IfPlayerExistsIsGlobalPlayers(string playerName)
        {
            return GlobalPlayersCollection.Any(x => x.Name.Equals(playerName));
        }

        #endregion

        #region Events

        public void PlayerJoinedEvent(string data)
        {
            data = data.Split("INFO]: ", StringSplitOptions.RemoveEmptyEntries)[1];
            var playerName = data.Split('[')[0].Trim();

            Logger.Write($"PlayerJoinedEvent: Triggered by string: '{data}'. Data retrieved: '{playerName}'.");

            if (!IfPlayerExistsIsGlobalPlayers(playerName))
            {
                Logger.Write($"{playerName} could not be found in the global players list. Refreshing usercache.json.");
                if (LoadPlayerDataFromFileCommand.CanExecute("usercache.json"))
                    LoadPlayerDataFromFileCommand.Execute("usercache.json");
            }

            foreach (var globalPlayer in GlobalPlayersCollection.Where(x => x.Name.Equals(playerName)).Where(x => !x.IsOnline))
                globalPlayer.IsOnline = true;
        }

        public void PlayerLeaveEvent(string data)
        {
            data = data.Split("INFO]: ", StringSplitOptions.RemoveEmptyEntries)[1];
            var playerName = data.Split(" lost connection:", StringSplitOptions.RemoveEmptyEntries)[0].Trim();

            Logger.Write($"PlayerLeaveEvent: Triggered by string: '{data}'. Data retrieved: '{playerName}'.");

            if (!IfPlayerExistsIsGlobalPlayers(playerName))
            {
                Logger.Write($"{playerName} could not be found in the global players list. Reloading usercache.json file.");
                if (LoadPlayerDataFromFileCommand.CanExecute("usercache.json"))
                    LoadPlayerDataFromFileCommand.Execute("usercache.json");
            }

            foreach (var globalPlayer in GlobalPlayersCollection.Where(x => x.Name.Equals(playerName)).Where(x => x.IsOnline))
                globalPlayer.IsOnline = false;
        }

        public void PlayerOpEvent(string data)
        {
            Logger.Write($"PlayerOpEvent: Triggered by string: '{data}'. Reloading ops.json file.");

            if (LoadPlayerDataFromFileCommand.CanExecute("ops.json"))
                LoadPlayerDataFromFileCommand.Execute("ops.json");
        }

        public void PlayerBanEvent(string data)
        {
            Logger.Write($"PlayerBanEvent: Triggered by string: '{data}'. Reloading banned-players.json file.");

            if (LoadPlayerDataFromFileCommand.CanExecute("banned-players.json"))
                LoadPlayerDataFromFileCommand.Execute("banned-players.json");
        }

        public void PlayerWhitelistEvent(string data)
        {
            Logger.Write($"PlayerWhitelistEvent: Triggered by string: '{data}'. Reloading whitelist.json file.");

            if (LoadPlayerDataFromFileCommand.CanExecute("whitelist.json"))
                LoadPlayerDataFromFileCommand.Execute("whitelist.json");
        }

        public void PlayerListEvent(string data, bool startup = false)
        {
            var regex = Regex.Match(data, @"There are (\d+)\/(\d+) players online:");

            if (!regex.Success) return;

            int playersOnline;
            if (int.TryParse(regex.Groups[1].Value, out playersOnline))
                ServerConfigViewModel.Instance.Server.PlayersOnline = playersOnline;

            int maxPlayers;
            if (int.TryParse(regex.Groups[2].Value, out maxPlayers))
                ServerConfigViewModel.Instance.Server.MaxPlayers = maxPlayers;

            Logger.Write($"PlayerListEvent: Triggered by string: '{data}'. Data retrieved; Players online: '{playersOnline}'. Max Players: '{maxPlayers}'");
        }

        #endregion
    }
}
