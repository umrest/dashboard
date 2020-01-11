using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardJoystickData :  JoystickData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    
        public override byte[] Serialize()
        {
            byte[] ret = new byte[128];

            ret[TYPE_OFFSET] = TYPE;


            BitArray8 button_data_1 = new BitArray8();
            BitArray8 button_data_2 = new BitArray8();

            button_data_1.SetBit(0, button_a);
            button_data_1.SetBit(1, button_b);
            button_data_1.SetBit(2, button_x);
            button_data_1.SetBit(3, button_y);
            button_data_1.SetBit(4, button_lb);
            button_data_1.SetBit(5, button_rb);
            button_data_1.SetBit(6, button_start);
            button_data_1.SetBit(7, button_select);

            button_data_2.SetBit(0, button_lj);
            button_data_2.SetBit(1, button_rj);
            
            ret[BUTTONS_1_OFFSET] = button_data_1.aByte;
            ret[BUTTONS_2_OFFSET] = button_data_2.aByte;

            BitConverter.GetBytes(lj_x).CopyTo(ret, LJ_X_OFFSET);
            BitConverter.GetBytes(lj_y).CopyTo(ret, LJ_Y_OFFSET);
            BitConverter.GetBytes(rj_x).CopyTo(ret, RJ_X_OFFSET);
            BitConverter.GetBytes(rj_y).CopyTo(ret, RJ_Y_OFFSET);
            BitConverter.GetBytes(lt).CopyTo(ret, LT_OFFSET);
            BitConverter.GetBytes(rt).CopyTo(ret, RT_OFFSET);

            return ret;
        }

        public override void Deserialize(byte[] data)
        {
            throw new NotImplementedException();
        }

        private byte joy2byte(int joy)
        {
            byte ret = (byte)((joy / 65535.0 * 2) * 127);
            return ret;
        }

        public void Load(JoystickState state)
        {
            button_a = state.GetButtons()[0];
            button_b = state.GetButtons()[1];
            button_x = state.GetButtons()[2];
            button_y = state.GetButtons()[3];
            
            button_lb = state.GetButtons()[4];
            button_rb = state.GetButtons()[5];
            button_select = state.GetButtons()[6];
            button_start = state.GetButtons()[7];
            button_lj = state.GetButtons()[8];
            button_rj = state.GetButtons()[9];
            
            lj_x = joy2byte(state.X);
            lj_y = joy2byte(state.Y);
            rj_x = joy2byte(state.RotationX);
            rj_y = joy2byte(state.RotationY);

            rt = joy2byte(state.Z);
            lt = joy2byte(state.Z);

            
        }
    }
}
