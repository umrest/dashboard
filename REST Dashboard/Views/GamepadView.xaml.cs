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
    /// Interaction logic for GamepadView.xaml
    /// </summary>
    public partial class GamepadView : UserControl
    {
        public GamepadView()
        {
            InitializeComponent();

            StateData.joystick_data.PropertyChanged += Joystick_data_PropertyChanged;
        }

        private void Joystick_data_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            update_joysticks();
        }

        private void update_joysticks()
        {
            left_joystick.SetX(StateData.joystick_data.lj_x);
            left_joystick.SetY(StateData.joystick_data.lj_y);

            right_joystick.SetX(StateData.joystick_data.rj_x);
            right_joystick.SetY(StateData.joystick_data.rj_y);
        }
    }
}
