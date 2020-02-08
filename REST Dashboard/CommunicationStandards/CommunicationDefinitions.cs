using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard.CommunicationStandards
{
    public static class CommunicationDefinitions
    {
        public enum TYPE : byte
        {
            JOYSTICK = 1,
            VISION = 2,
            REALSENSE = 3,
            ROBOT_STATE = 11,
            SENSOR_STATE = 10,
            DATAAGGREGATOR_STATE = 8,
            DASHBOARD_DATA = 9,

            VISION_COMMAND = 12,
            VISION_IMAGE = 13,
            INDENTIFIER = 250

        };

        public enum IDENTIFIER : byte
        {
            DASHBOARD = 1,
            VISION = 2,
            TCPSERIAL = 3,
            HERO = 4
        }

        public static Dictionary<TYPE, int> PACKET_SIZES = new Dictionary<TYPE, int>(){
            {TYPE.JOYSTICK,  8},
            {TYPE.VISION,   36},
            {TYPE.REALSENSE,  127},
            {TYPE.DATAAGGREGATOR_STATE,  127},
            {TYPE.DASHBOARD_DATA,  9},
            {TYPE.SENSOR_STATE,  120},
            {TYPE.ROBOT_STATE,  1},

            {TYPE.VISION_COMMAND,  127},
            {TYPE.VISION_IMAGE,  8191},
            {TYPE.INDENTIFIER,  127},
        };

        public static byte[] key = { 44, 254, 153 };
    }
}
