using System;
using System.Xml;
using HrtzCraft.Utilities;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace HrtzCraft.Views
{
    /// <summary>
    /// Interaction logic for ConsoleView.xaml
    /// </summary>
    public partial class ConsoleView
    {
        public ConsoleView()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                try
                {
                    using (var reader = XmlReader.Create("mcSyntax.xshd"))
                        TextBoxConsoleOutput.SyntaxHighlighting = HighlightingLoader.Load(reader,
                            HighlightingManager.Instance);
                }
                catch (Exception ex)
                {
                    Logger.Write("Could load file mcSyntax.xshd for syntax highlighting", true, ex);
                }

                if (TextBoxConsoleOutput.IsVisible)
                    TextBoxConsoleOutput.ScrollToEnd();
            };
        }

        private void TextBoxConsoleOutput_OnTextChanged(object sender, EventArgs e)
        {
            if (TextBoxConsoleOutput.IsVisible && !TextBoxConsoleInput.IsMouseOver)
                TextBoxConsoleOutput.ScrollToEnd();
        }
    }
}
