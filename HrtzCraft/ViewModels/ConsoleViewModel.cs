using System.Collections.Generic;
using System.Windows.Input;
using HrtzCraft.Extensions;
using HrtzCraft.Interfaces;
using HrtzCraft.Relays;

namespace HrtzCraft.ViewModels
{
    public class ConsoleViewModel : ObservableObject, IPageViewModel
    {
        public ConsoleViewModel()
        {
            RememberedCommandsCollection = new List<string>();
        }

        private ICommand _addToRememberedCommandsCommand;

        public string Name => "Console";
        public List<string> RememberedCommandsCollection { get; set; }

        public ICommand AddToRememberedCommandsCommand
        {
            get
            {
                return _addToRememberedCommandsCommand ??
                       (_addToRememberedCommandsCommand = new RelayCommand(p => Execute_AddToRememberedCommands(p as string), p => p is string));
            }
        }

        private void Execute_AddToRememberedCommands(string value)
        {
            RememberedCommandsCollection.Add(value);
        }
    }
}
