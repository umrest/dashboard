

namespace REST_Dashboard
{
    public abstract class DataAggregatorState : RESTPacket
    {
        public DataAggregatorState()
        {

        }

        public new byte TYPE = 8;

        public bool hero_connected;
        public bool vision_connected;
        public bool tcpserial_connected;
        public bool realsense_connected;
        public bool dashboard_connected;


        // Offsets

        protected const int STATUS_OFFSET = 1;

    }
}
