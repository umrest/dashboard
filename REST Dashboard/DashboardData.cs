﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardData
    {

        public DashboardData()
        {

        }

        public enum RobotStateEnum : byte
        {
            Teleop=0,
            Auton=1
        };

        public bool enabled = false;
        public bool estop = false;
        public RobotStateEnum robot_state;


        public byte[] Serialize()
        {
            byte[] ret = new byte[128];
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    byte type = 9;

                    writer.Write(type);

                    byte[] enabled_byte = new byte[8];

                    for(int i = 0; i < 8; i++)
                    {
                        if (enabled && !estop)
                        {
                            enabled_byte[i] = (byte)(20 + i);
                        }
                        else
                        {
                            enabled_byte[i] = 0;
                        }
                    }

                    writer.Write(enabled_byte);
                }

                m.ToArray().CopyTo(ret, 0);
                return ret;
            }
        }
    }
}
