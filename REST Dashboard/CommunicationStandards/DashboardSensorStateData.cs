﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardSensorStateData : SensorStateData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public DashboardMotorInfo[] motor_info;
        public GyroData gyro_data;

        public DashboardSensorStateData() : base()
        {
            motor_info = new DashboardMotorInfo[14];

            for (int i = 0; i < 14; i++)
            {
                motor_info[i] = new DashboardMotorInfo();
            }
            
        }
        public override byte[] Serialize()
        {
            throw new NotFiniteNumberException();
        }

        public override void Deserialize(byte[] data)
        {
            for (int i = 0; i < 14; i++)
            {
                motor_info[i].Deserialize(data.Skip(MOTOR_INFO_OFFSET + i * MotorInfo.MOTOR_INFO_SIZE).Take(MotorInfo.MOTOR_INFO_SIZE).ToArray());
            }

            PropertyChanged(this, new PropertyChangedEventArgs(null));
        }
    }

    public class DashboardMotorInfo : MotorInfo, INotifyPropertyChanged
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

        public DashboardMotorInfo()
        {
            
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

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
        public void Deserialize(byte[] data)
        {
            can_id = data[MOTOR_INFO_CAN_OFFSET];
            current = BitConverter.ToInt16(data, MOTOR_INFO_CURRENT_OFFSET) / 100.0;
            position = BitConverter.ToInt64(data, MOTOR_INFO_POSITION_OFFSET);
            speed = BitConverter.ToInt32(data, MOTOR_INFO_VELOCITY_OFFSET);

            percentage = data[MOTOR_INFO_PERCENTAGE_OFFSET] - 127;


            PropertyChanged(this, new PropertyChangedEventArgs(null));

        }
    }
}
