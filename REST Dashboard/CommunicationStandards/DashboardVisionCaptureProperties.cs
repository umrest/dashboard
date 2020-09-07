using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using comm;

namespace REST_Dashboard
{
    public class DashboardVisionCaptureProperties : comm.Vision_Properties, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public uint exposure
        {
            get
            {
                return get_exposure();
            }
            set
            {
                PropertyChanged(this, new PropertyChangedEventArgs("exposure"));
                set_exposure(value);
            }
        }

        public uint gain
        {
            get
            {
                return get_gain();
            }
            set
            {
                PropertyChanged(this, new PropertyChangedEventArgs("gain"));
                set_gain(value);
            }
        }

    }

}
