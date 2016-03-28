using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Windows.Input;
using System.Xml;
using HrtzCraft.Extensions;
using HrtzCraft.Interfaces;
using HrtzCraft.Models;
using HrtzCraft.Relays;
using HrtzCraft.Utilities;

namespace HrtzCraft.ViewModels
{
    public class BuildToolsViewModel : ObservableObject, IPageViewModel
    {
        public BuildToolsViewModel()
        {
            if (LoadBuildToolsConfigFromFileCommand.CanExecute(BuildToolsSettingsFullPath))
                LoadBuildToolsConfigFromFileCommand.Execute(BuildToolsSettingsFullPath);

            if (string.IsNullOrEmpty(BuildTool.BuildToolsRssUri))
                BuildTool.BuildToolsRssUri = "https://hub.spigotmc.org/jenkins/job/BuildTools/rssAll";

            if (string.IsNullOrEmpty(BuildTool.BuildToolsUri))
                BuildTool.BuildToolsRssUri = "https://hub.spigotmc.org/jenkins/job/BuildTools/lastSuccessfulBuild/artifact/target/BuildTools.jar";

            if (string.IsNullOrEmpty(BuildTool.SpigotVersion))
                BuildTool.BuildToolsRssUri = "1.9";

            if (BuildTool.BuildToolsDirectory != null && !Directory.Exists(BuildTool.BuildToolsDirectory))
            {
                Directory.CreateDirectory(BuildTool.BuildToolsDirectory);
                Logger.Write($"Created directory: {BuildTool.BuildToolsDirectory}", true);
            }
        }

        private string _consoleOutput;
        private ICommand _installBuildToolsCommand;
        private ICommand _runBuildToolsCommand;
        private ICommand _saveBuildToolsConfigToFileCommand;
        private ICommand _loadBuildToolsConfigFromFileCommand;

        public string Name => "BuildTools";
        public static BuildToolsViewModel Instance { get; } = new BuildToolsViewModel();
        public BuildTool BuildTool { get; set; } = new BuildTool();
        public string BuildToolsSettingsFullPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "buildtools.xml");

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

        public ICommand InstallBuildToolsCommand
        {
            get
            {
                return _installBuildToolsCommand ??
                       (_installBuildToolsCommand = new RelayCommand(Execute_InstallBuildTools, p => true));
            }
        }

        public ICommand RunBuildToolsCommand
        {
            get
            {
                return _runBuildToolsCommand ??
                       (_runBuildToolsCommand = new RelayCommand(Execute_RunBuildTools, p => Directory.Exists(BuildTool.BuildToolsDirectory) && File.Exists(BuildTool.GitBashFullName)));
            }
        }

        public ICommand SaveBuildToolsConfigToFileCommand
        {
            get
            {
                return _saveBuildToolsConfigToFileCommand ??
                       (_saveBuildToolsConfigToFileCommand = new RelayCommand(p => Execute_SaveBuildToolsConfigToFile(p as string), p => p is string));
            }
        }

        public ICommand LoadBuildToolsConfigFromFileCommand
        {
            get
            {
                return _loadBuildToolsConfigFromFileCommand ??
                       (_loadBuildToolsConfigFromFileCommand = new RelayCommand(p => Execute_LoadBuildToolsConfigFromFile(p as string), p => p is string && File.Exists(p.ToString())));
            }
        }

        private async void Execute_RunBuildTools(object obj)
        {
            var argsBuilder = new StringBuilder();
            argsBuilder.Append("--login -i -c \"java -jar \"\"BuildTools.jar\"\"\"");

            if (!string.IsNullOrEmpty(BuildTool.SpigotVersion))
                argsBuilder.Append(" --rev " + BuildTool.SpigotVersion);

            using (var process = new Process())
            {
                process.StartInfo.FileName = BuildTool.GitBashFullName;
                process.StartInfo.WorkingDirectory = BuildTool.BuildToolsDirectory;
                process.StartInfo.Arguments = argsBuilder.ToString();
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.ErrorDataReceived += (sender, args) =>
                {
                    if (args.Data == null) return;
                    ConsoleOutput += (args.Data + Environment.NewLine);
                };

                process.OutputDataReceived += (sender, args) =>
                {
                    if (args.Data == null) return;
                    ConsoleOutput += (args.Data + Environment.NewLine);
                };

                process.Exited += (sender, args) =>
                {
                    var builtFilePath = Path.Combine(BuildTool.BuildToolsDirectory, @"Spigot\Spigot-Server\target");

                    if (Directory.Exists(builtFilePath))
                        Process.Start(builtFilePath);
                };

                process.Start();

                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

                await process.WaitForExitAsync();
            }
        }

        private async void Execute_InstallBuildTools(object obj)
        {
            using (var webClient = new WebClient())
            {
                webClient.Proxy = null;

                webClient.DownloadProgressChanged += (sender, args) =>
                {
                    ConsoleOutput = $"Downloading latest BuildTools: {args.ProgressPercentage}%{Environment.NewLine}";
                };

                webClient.DownloadFileCompleted += (sender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        ConsoleOutput += args.Error.Message;
                        return;
                    }

                    SyndicationFeed feed;
                    using (var reader = XmlReader.Create(BuildTool.BuildToolsRssUri))
                        feed = SyndicationFeed.Load(reader);

                    if (feed == null)
                    {
                        ConsoleOutput += "Could not get the BuildTools RSS feed - Downloading latest build";
                        Logger.Write("Could not get the BuildTools RSS feed - Downloading latest build", true);
                        return;
                    }

                    var buildInfo = feed.Items.First().Title.Text;
                    var buildDate = feed.Items.First().LastUpdatedTime.ToLocalTime().ToString();

                    ConsoleOutput +=
                        $"Successfully downloaded {buildInfo ?? "(Unknown build version)"} ({buildDate})!";
                };

                await webClient.DownloadFileTaskAsync(BuildTool.BuildToolsUri, Path.Combine(BuildTool.BuildToolsDirectory, "BuildTools.jar"));
            }
        }

        private void Execute_SaveBuildToolsConfigToFile(string filename)
        {
            Serializer.SerializeToXml(BuildTool, filename);
        }

        private void Execute_LoadBuildToolsConfigFromFile(string filename)
        {
            BuildTool = Serializer.DeserializeFromXml<BuildTool>(filename);
        }
    }
}
