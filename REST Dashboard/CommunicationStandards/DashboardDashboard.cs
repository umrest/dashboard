using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardDashboard : comm.Dashboard
    {

        public DashboardDashboard()
        {
            
        }

        public override byte[] Serialize()
        {
            if (enabled && !estop)
            {
                _enabled_1 = (byte)20;
                _enabled_2 = (byte)21;
                _enabled_3 = (byte)22;
                _enabled_4 = (byte)23;
                _enabled_5 = (byte)24;
                _enabled_6 = (byte)25;
                _enabled_7 = (byte)26;
                _enabled_8 = (byte)27;
            }
            else
            {
                _enabled_1 = 0;
                _enabled_2 = 0;
                _enabled_3 = 0;
                _enabled_4 = 0;
                _enabled_5 = 0;
                _enabled_6 = 0;
                _enabled_7 = 0;
                _enabled_8 = 0;
            }

            _state = (byte)robot_state;

            return base.Serialize();
        }

        public enum RobotStateEnum : byte
        {
            Disabled=0,
            Teleop=1,
            Auton=2,
            Test=3
        };

        public bool enabled = false;
        public bool estop = false;
        public RobotStateEnum robot_state = RobotStateEnum.Teleop;
    }
}
