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
            _motor_info_2 = new DashboardMotor_Info();
            _motor_info_3 = new DashboardMotor_Info();
            _motor_info_4 = new DashboardMotor_Info();
            _motor_info_5 = new DashboardMotor_Info();
            _motor_info_6 = new DashboardMotor_Info();
            _motor_info_7 = new DashboardMotor_Info();
            _motor_info_8 = new DashboardMotor_Info();
            _motor_info_9 = new DashboardMotor_Info();
            _motor_info_10 = new DashboardMotor_Info();
        }
        public DashboardMotor_Info[] motor_info
        {
            get
            {
                DashboardMotor_Info[] ret = new DashboardMotor_Info[10];
                ret[0] = (DashboardMotor_Info)get_motor_info_1();
                ret[1] = (DashboardMotor_Info)get_motor_info_2();
                ret[2] = (DashboardMotor_Info)get_motor_info_3();
                ret[3] = (DashboardMotor_Info)get_motor_info_4();
                ret[4] = (DashboardMotor_Info)get_motor_info_5();
                ret[5] = (DashboardMotor_Info)get_motor_info_6();
                ret[6] = (DashboardMotor_Info)get_motor_info_7();
                ret[7] = (DashboardMotor_Info)get_motor_info_8();
                ret[8] = (DashboardMotor_Info)get_motor_info_9();
                ret[9] = (DashboardMotor_Info)get_motor_info_10();
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

        public int id
        {
            get
            {
                return get_can_id();
            }
        }

        public uint current
        {
            get
            {
                return get_current();
            }
        }

        public int percent
        {
            get
            {
                return get_percent();
            }
        }

        public uint position
        {
            get
            {
                return get_position();
            }
        }


        public uint velocity
        {
            get
            {
                return get_velocity();
            }
        }
        public string label { 
            get
            {
                if (CANID_MAP.ContainsKey(id))
                {
                    return CANID_MAP[id];
                }
                return "undefined";
            } 
        }
    }
}
