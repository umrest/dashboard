using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace REST_Dashboard
{
    /// <summary>
    /// Interaction logic for ConnectionIndicator.xaml
    /// </summary>
    public partial class ConnectionIndicator : UserControl
    {
        Color disconnected_color = Color.FromRgb(127, 0, 0);
        Color connected_color = Color.FromRgb(0, 127, 0);

        public ConnectionIndicator()
        {
            InitializeComponent();
        }
        bool _connected = false;
        public bool connected
        {
            set
            {
                _connected = value;
                ConnectionColor.Fill = new SolidColorBrush(_connected ? connected_color : disconnected_color);
            }
            get
            {
                return _connected;
            }
        }

        public string _label = "null";
        public string label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
                ConnectionLabel.Content = _label;
            }
        }

    }
}
