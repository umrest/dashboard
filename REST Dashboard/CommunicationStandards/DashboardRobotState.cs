using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static REST_Dashboard.DashboardData;

namespace REST_Dashboard
{
    public class DashboardRobotState :  RobotState
    {
        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(byte[] data)
        {
            robot_state = (RobotStateEnum)data[1];

            if(robot_state == RobotStateEnum.Disabled)
            {
                StateData.dashboard_state.enabled = false;
            }
            else
            {
                StateData.dashboard_state.enabled = true;
                StateData.dashboard_state.robot_state = robot_state;
            }

            StateData.mainwindow.update_indicators();
        }
    }
}
