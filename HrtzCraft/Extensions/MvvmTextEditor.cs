using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ICSharpCode.AvalonEdit;

namespace HrtzCraft.Extensions
{
    public class MvvmTextEditor : TextEditor, INotifyPropertyChanged
    {
        public static DependencyProperty DocumentTextProperty = DependencyProperty.Register("DocumentText", typeof(string), typeof(MvvmTextEditor),

            new PropertyMetadata((obj, args) =>
            {
                var target = (MvvmTextEditor) obj;
                target.DocumentText = (string) args.NewValue;
            })
        );

        public string DocumentText
        {
            get { return Text; }
            set { Text = value; }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            OnPropertyChanged();
            base.OnTextChanged(e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
