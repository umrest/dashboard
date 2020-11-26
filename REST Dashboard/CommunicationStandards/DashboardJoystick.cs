using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using comm;

namespace REST_Dashboard
{
    public class DashboardJoystick :  comm.Joystick, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public double lj_x
        {
            get
            {
                return get_lj_x();
            }
        }
        public double lj_y
        {
            get
            {
                return get_lj_y();
            }
        }
        public double rj_x
        {
            get
            {
                return get_rj_x();
            }
        }
        public double rj_y
        {
            get
            {
                return get_rj_y();
            }
        }

        private double joy2double(int joy)
        {
            return (joy / 65535.0) * 2;
        }

        public void Load(JoystickState state)
        {
            set_button_A(state.GetButtons()[0]);
            set_button_B(state.GetButtons()[1]);
            set_button_X(state.GetButtons()[2]);
            set_button_Y(state.GetButtons()[3]);
            set_button_LB(state.GetButtons()[4]);
            set_button_RB(state.GetButtons()[5]);
            set_button_Select(state.GetButtons()[6]);
            set_button_Start(state.GetButtons()[7]);
            set_button_LJ(state.GetButtons()[8]);
            set_button_RJ(state.GetButtons()[9]);

            set_lj_x(joy2double(state.X));
            set_lj_y(joy2double(state.Y));
            set_rj_x(joy2double(state.RotationX));
            set_rj_y(joy2double(state.RotationY));

            set_rt(joy2double(state.Z));
            set_lt(joy2double(state.Z));

            int[] all_pov = state.GetPointOfViewControllers();
            int pov = all_pov[0];

            if (pov != -1)
            {
                pov /= 100;

                set_button_POVU(pov == 0 || pov == 45 || pov == 315);
                set_button_POVD(pov == 180 || pov == 225 || pov == 135);
                set_button_POVL(pov == 270 || pov == 315 || pov == 225);
                set_button_POVR(pov == 90 || pov == 45 || pov == 135);
            }
            else
            {
                set_button_POVU(false);
                set_button_POVD(false);
                set_button_POVL(false);
                set_button_POVR(false);
            }
            
            PropertyChanged(this, new PropertyChangedEventArgs(null));

            
        }
    }
}
