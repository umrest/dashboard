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
    public class DashboardSensor_State : comm.Sensor_State, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public DashboardSensor_State()
        {
            _motor_info_1 = new DashboardMotor_Info();
        }
        public DashboardMotor_Info[] motor_info
        {
            get
            {
                DashboardMotor_Info[] ret = new DashboardMotor_Info[14];
                ret[0] = (DashboardMotor_Info)get_motor_info_1();
                return ret;
            }
        }
    }

    public class DashboardMotor_Info : comm.Motor_Info, INotifyPropertyChanged
    {
        private static Dictionary<int, string> CANID_MAP = new Dictionary<int, string>()
        {
            { 1, "FrontLeftWheel" },
            { 2, "FrontRightWheel" },
            { 3, "BackLeftWheel" },
            { 4, "BackRightWheel" },
            { 11, "LeftActuator" },
            { 12, "RightActuator" },
            { 13, "AugerRotation"},
            { 14, "AugerExtender"},
            { 21, "LeftDumper" },
            { 22, "RightDumper"}
, 

        };

        public DashboardMotor_Info()
        {
            
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public int can_id
        {
            get
            {
                return get_can_id();
            }
        }

        public string label { 
            get
            {
                if (CANID_MAP.ContainsKey(can_id))
                {
                    return CANID_MAP[can_id];
                }
                return "undefined";
            } 
        }
    }
}
