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

namespace REST_Dashboard.Views
{
    /// <summary>
    /// Interaction logic for ButtonIndicator.xaml
    /// </summary>
    public partial class ButtonIndicator : UserControl
    {
        public ButtonIndicator()
        {
            InitializeComponent();
        }

        public void SetPressed(bool pressed)
        {
            rect.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (pressed)
                {
                    rect.Fill = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    rect.Fill = new SolidColorBrush(Colors.White);
                }
            }
            ));
           
        }
    }
}
