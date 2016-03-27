using System.Windows;

namespace HrtzCraft.Controls
{
    /// <summary>
    /// Interaction logic for ServerSettingBox.xaml
    /// </summary>
    public partial class ServerSettingBox
    {
        public ServerSettingBox()
        {
            InitializeComponent();
        }

        public object DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register("DisplayName", typeof(string),
            typeof(ServerSettingBox), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof (int),
            typeof (ServerSettingBox), new PropertyMetadata(0));
    }
}
