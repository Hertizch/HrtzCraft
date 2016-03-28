using System;

namespace HrtzCraft.Views
{
    /// <summary>
    /// Interaction logic for BuildToolsView.xaml
    /// </summary>
    public partial class BuildToolsView
    {
        public BuildToolsView()
        {
            InitializeComponent();

            if (TextBoxConsoleOutput.IsVisible)
                TextBoxConsoleOutput.ScrollToEnd();
        }

        private void TextBoxConsoleOutput_OnTextChanged(object sender, EventArgs e)
        {
            if (TextBoxConsoleOutput.IsVisible)
                TextBoxConsoleOutput.ScrollToEnd();
        }
    }
}
