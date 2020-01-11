using REST_Dashboard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.DirectInput;

namespace REST_Dashboard
{
    public static class StateData
    {

        public static DashboardData dashboard_state = new DashboardData();


        public static DashboardVisionData vision_data = new DashboardVisionData();
        public static DashboardRobotStateData robot_state_data = new DashboardRobotStateData();

        public static DashboardTagPosition t0 = (DashboardTagPosition)vision_data.t0;
        public static DashboardTagPosition t1 = (DashboardTagPosition)vision_data.t1;

        public static DashboardJoystickData joystick_data = new DashboardJoystickData();

        public static DashboardDataAggregatorState dataaggregator_state = new DashboardDataAggregatorState();

        public static DashboardVisionCaptureProperties properties = new DashboardVisionCaptureProperties();

        public static Guid joy_guid;

        public static GlobalHotkey space_hotkey;

        public static bool send_joystick_enabled = false;

        public static List<DeviceInstance> get_joysticks()
        {
            return Input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly).ToList();
        }

        public static DirectInput Input = new DirectInput();
    }
}
