using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpigotMinecraftControl.Annotations;

namespace HrtzCraft.Extensions
{
    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Executes the property changed event
        /// </summary>
        /// <param name="propertyName">Property name to listen for changes</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
