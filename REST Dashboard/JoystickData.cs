

namespace REST_Dashboard
{
    public abstract class JoystickData
    {
        public JoystickData()
        {

        }

        public bool button_a;
        public bool button_b;
        public bool button_x;
        public bool button_y;

        public bool button_rb;
        public bool button_lb;
        public bool button_select;
        public bool button_start;

        public bool button_lj;
        public bool button_rj;

        public byte lj_x;
        public byte lj_y;
        public byte rj_x;
        public byte rj_y;
        public byte lt;
        public byte rt;

        public abstract byte[] Serialize();

        protected const int TYPE_OFFSET = 0;
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
