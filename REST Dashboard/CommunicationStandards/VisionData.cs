

namespace REST_Dashboard
{
    public abstract class VisionData : RESTPacket
    {
        public VisionData()
        {

        }

        public new byte TYPE = 2;

        public short yaw { get; set; }
        public short pitch { get; set; }
        public short roll { get; set; }

        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }


        // Offsets

        protected const int BUTTONS_1_OFFSET = 1;
        protected const int BUTTONS_2_OFFSET = 2;
        protected const int LJ_X_OFFSET = 3;
        protected const int LJ_Y_OFFSET = 7;
        protected const int RJ_X_OFFSET = 11;
        protected const int RJ_Y_OFFSET = 15;
        protected const int LT_OFFSET = 19;
        protected const int RT_OFFSET = 23;
    }
}
