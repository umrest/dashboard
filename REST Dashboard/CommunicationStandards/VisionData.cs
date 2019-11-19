

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

        protected const int YAW_OFFSET = 1;
        protected const int PITCH_OFFSET = 3;
        protected const int ROLL_OFFSET = 5;
        protected const int X_OFFSET = 7;
        protected const int Y_OFFSET = 9;
        protected const int Z_OFFSET = 11;
    }
}
