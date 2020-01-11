using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardVisionCaptureProperties : VisionCaptureProperties, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public double exposure
        {
            get
            {
                return _exposure;
            }
            set
            {
                PropertyChanged(this, new PropertyChangedEventArgs("exposure"));
                _exposure = value;
            }
        }

        public byte gain
        {
            get
            {
                return _gain;
            }
            set
            {
                PropertyChanged(this, new PropertyChangedEventArgs("gain"));
                _gain = value;
            }
        }
        public DashboardVisionCaptureProperties() : base()
        {

        }
        public override byte[] Serialize()
        {
            byte[] ret = new byte[128];

            ret[0] = TYPE;

            BitConverter.GetBytes((short)(exposure * 100)).CopyTo(ret, EXPOSURE_OFFSET);
            BitConverter.GetBytes(gain).CopyTo(ret, GAIN_OFFSET);

            return ret;
        }

        public override void Deserialize(byte[] data)
        {
            
        }
    }

}
