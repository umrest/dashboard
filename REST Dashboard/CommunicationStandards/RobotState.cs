

using static REST_Dashboard.DashboardData;

namespace REST_Dashboard
{
    public abstract class RobotState : RESTPacket
    {
        public RobotState()
        {

        }

        public new byte TYPE = 11;

        public RobotStateEnum robot_state = RobotStateEnum.Teleop;


        // Offsets

        protected const int STATUS_OFFSET = 1;

    }
}
