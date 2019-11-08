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

        public bool connected
        {
            set
            {
                ConnectionColor.Fill = new SolidColorBrush(value ? connected_color : disconnected_color);
            }
        }

        public string label
        {
            set
            {
                ConnectionLabel.Content = value;
            }
        }
    }
}
