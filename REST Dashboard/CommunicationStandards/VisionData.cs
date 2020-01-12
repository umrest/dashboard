

namespace REST_Dashboard
{
    public abstract class VisionData : RESTPacket
    {
        public VisionData()
        {

        }

        public new byte TYPE = 2;

        public TagPosition t0;
        public TagPosition t1;
        public FieldPosition fp;


      

        protected const int TAG_0_OFFSET = 1;
        protected const int TAG_1_OFFSET = 13;
        protected const int FIELD_POSITION_OFFSET = TAG_1_OFFSET + 12;


    }

    public abstract class TagPosition
    {
        // Offsets

        public short yaw { get; set; }
        public short pitch { get; set; }
        public short roll { get; set; }

        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }

        protected const int YAW_OFFSET = 0;
        protected const int PITCH_OFFSET = 2;
        protected const int ROLL_OFFSET = 4;
        protected const int X_OFFSET = 6;
        protected const int Y_OFFSET = 8;
        protected const int Z_OFFSET = 10;

        public abstract void Deserialize(byte[] data);
    }

    public abstract class FieldPosition
    {
        // Offsets

        public short yaw { get; set; }

        public short X { get; set; }
        public short Y { get; set; }

        protected const int YAW_OFFSET = 0;
        protected const int X_OFFSET = 2;
        protected const int Y_OFFSET = 4;

        public abstract void Deserialize(byte[] data);
    }
}
