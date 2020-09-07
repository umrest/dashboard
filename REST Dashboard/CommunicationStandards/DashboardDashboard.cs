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
