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
            left_joystick.SetX(StateData.joystick_data.get_lj_x());
            left_joystick.SetY(StateData.joystick_data.get_lj_y());

            right_joystick.SetX(StateData.joystick_data.get_rj_x());
            right_joystick.SetY(StateData.joystick_data.get_rj_y());

            LB_indicator.SetPressed(StateData.joystick_data.get_button_LB());
            RB_Indicator.SetPressed(StateData.joystick_data.get_button_RB());
            u_Indicator.SetPressed(StateData.joystick_data.get_button_POVU());
            d_Indicator.SetPressed(StateData.joystick_data.get_button_POVD());
            l_Indicator.SetPressed(StateData.joystick_data.get_button_POVL());
            r_Indicator.SetPressed(StateData.joystick_data.get_button_POVR());
            a_Indicator.SetPressed(StateData.joystick_data.get_button_A());
            b_Indicator.SetPressed(StateData.joystick_data.get_button_B());
            x_Indicator.SetPressed(StateData.joystick_data.get_button_X());
            y_Indicator.SetPressed(StateData.joystick_data.get_button_Y());
            start_Indicator.SetPressed(StateData.joystick_data.get_button_Start());
            select_Indicator.SetPressed(StateData.joystick_data.get_button_Select());

        }
    }
}
