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
    /// Interaction logic for JoystickView.xaml
    /// </summary>
    public partial class JoystickView : UserControl
    {
        public JoystickView()
        {
            InitializeComponent();

            SetX(0);
            SetY(0);
        }


        public void SetX(double x)
        {
            int X_OFFSET = 40;
            Stick.Dispatcher.BeginInvoke((Action)(() => Canvas.SetLeft(Stick, X_OFFSET + x * 40)));
        }

        public void SetY(double y)
        {
            int Y_OFFSET = 40;
            Stick.Dispatcher.BeginInvoke((Action)(() => Canvas.SetTop(Stick, Y_OFFSET + y * 40)));
        }
    }
}
