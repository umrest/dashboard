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
            ROBOT_STATE = 10,
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
    }
}
