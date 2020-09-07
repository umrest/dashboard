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

        public static DashboardDashboard dashboard_state = new DashboardDashboard();


        public static DashboardVision vision = new DashboardVision();
        public static DashboardSensor_State sensor_state = new DashboardSensor_State();
        public static DashboardRobot_State robot_state = new DashboardRobot_State();

        public static DashboardJoystick joystick_data = new DashboardJoystick();

        public static DashboardData_Server data_server = new DashboardData_Server();

        public static DashboardVisionCaptureProperties properties = new DashboardVisionCaptureProperties();

        public static DashboardRealsense realsense = new DashboardRealsense();

        public static Guid joy_guid;

        public static GlobalHotkey space_hotkey;

        public static bool send_joystick_enabled = false;

        public static MainWindow mainwindow = null;

        public static List<DeviceInstance> get_joysticks()
        {
            return Input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly).ToList();
        }

        public static DirectInput Input = new DirectInput();
    }
}
