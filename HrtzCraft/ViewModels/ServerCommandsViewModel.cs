using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HrtzCraft.Enums;
using HrtzCraft.Extensions;
using HrtzCraft.Models;

namespace HrtzCraft.ViewModels
{
    public class ServerCommandsViewModel : ObservableObject
    {
        public ServerCommandsViewModel()
        {
            ServerControlViewModel.Instance.ConsoleOutput = "";

            ServerCommandsCollection = new ObservableCollection<ServerCommand>
            {
                // Spigot
                new ServerCommand
                {
                    DisplayName = "Show average TPS",
                    Command = "tps",
                    BukkitPermission = "bukkit.command.restart",
                    ServerCommandTarget = ServerCommandTarget.Spigot,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Operators
                },
                new ServerCommand
                {
                    DisplayName = "Enable benchmark timings",
                    Command = "timings on",
                    BukkitPermission = "bukkit.command.tps",
                    ServerCommandTarget = ServerCommandTarget.Spigot,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Operators
                },
                new ServerCommand
                {
                    DisplayName = "Disable benchmark timings",
                    Command = "timings off",
                    BukkitPermission = "bukkit.command.timings",
                    ServerCommandTarget = ServerCommandTarget.Spigot,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Operators
                },
                new ServerCommand
                {
                    DisplayName = "Merged benchmark timings",
                    Command = "timings merged",
                    BukkitPermission = "bukkit.command.timings",
                    ServerCommandTarget = ServerCommandTarget.Spigot,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Operators
                },
                new ServerCommand
                {
                    DisplayName = "Separate benchmark timings",
                    Command = "timings separate",
                    BukkitPermission = "bukkit.command.timings",
                    ServerCommandTarget = ServerCommandTarget.Spigot,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Operators
                },
                new ServerCommand
                {
                    DisplayName = "Paste benchmark timings",
                    Command = "timings paste",
                    BukkitPermission = "bukkit.command.timings",
                    ServerCommandTarget = ServerCommandTarget.Spigot,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Operators
                },
                new ServerCommand
                {
                    DisplayName = "Reset benchmark timings",
                    Command = "timings reset",
                    BukkitPermission = "bukkit.command.timings",
                    ServerCommandTarget = ServerCommandTarget.Spigot,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Operators
                },

                // Bukkit
                new ServerCommand
                {
                    DisplayName = "Print server version",
                    Command = "version",
                    BukkitPermission = "bukkit.command.version",
                    ServerCommandTarget = ServerCommandTarget.Bukkit,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Everybody
                },
                new ServerCommand
                {
                    DisplayName = "List all plugins",
                    Command = "plugins",
                    BukkitPermission = "bukkit.command.plugins",
                    ServerCommandTarget = ServerCommandTarget.Bukkit,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Everybody
                },
                new ServerCommand
                {
                    DisplayName = "Reload all plugins",
                    Command = "reload",
                    BukkitPermission = "bukkit.command.reload",
                    ServerCommandTarget = ServerCommandTarget.Bukkit,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Operators
                },
                new ServerCommand
                {
                    DisplayName = "Record all plugin event timings",
                    Command = "timings",
                    BukkitPermission = "bukkit.command.timings",
                    ServerCommandTarget = ServerCommandTarget.Bukkit,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Operators
                },

                // Vanilla
                new ServerCommand
                {
                    DisplayName = "Give achievement",
                    Command = "achievement give",
                    BukkitPermission = "bukkit.command.achievement",
                    MojangPermission = "minecraft.command.achievement",
                    ServerCommandTarget = ServerCommandTarget.Vanilla,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Operators
                },
                new ServerCommand
                {
                    DisplayName = "",
                    Command = "",
                    BukkitPermission = "bukkit.command.",
                    MojangPermission = "minecraft.command.",
                    ServerCommandTarget = ServerCommandTarget.Vanilla,
                    ServerCommandPermissionDefault = ServerCommandPermissionDefault.Operators
                },
            };

        }

        // Public fields
        public ObservableCollection<ServerCommand> ServerCommandsCollection { get; set; }
    }
}
