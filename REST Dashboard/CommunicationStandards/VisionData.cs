

namespace REST_Dashboard
{
    public abstract class RealsenseData : RESTPacket
    {
        public RealsenseData()
        {

        }

        public new byte TYPE = 3;

        public Obstacle o1;
        public Obstacle o2;
        public Obstacle o3;
        public Obstacle o4;


        protected const int OBSTACLE_1_OFFSET = 1;
        protected const int OBSTACLE_2_OFFSET = 17;
        protected const int OBSTACLE_3_OFFSET = 33;
        protected const int OBSTACLE_4_OFFSET = 49;

    }

    public abstract class Obstacle
    {
        // Offsets

        public short X { get; set; }
        public short Y { get; set; }
        public short width { get; set; }
        public short height { get; set; }

        protected const int X_OFFSET = 0;
        protected const int Y_OFFSET = 2;
        protected const int WIDTH_OFFSET = 4;
        protected const int HEIGHT_OFFSET = 6;

        public abstract void Deserialize(byte[] data);
    }
}
