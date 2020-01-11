

namespace REST_Dashboard
{
    public abstract class VisionCaptureProperties : RESTPacket
    {
        public VisionCaptureProperties()
        {

        }

        public new byte TYPE = 14;

        public double _exposure;
        public bool _exposure_auto;
        public byte _gain;

        public static int EXPOSURE_OFFSET = 1;
        public static int GAIN_OFFSET = 5;


    }

}
