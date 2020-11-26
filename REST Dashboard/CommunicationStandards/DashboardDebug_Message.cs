using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardDebug_Message : comm.Debug_Message, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public override void Deserialize(byte[] data)
        {
            base.Deserialize(data);
            PropertyChanged(this, new PropertyChangedEventArgs(null));
        }

        public DashboardDebug_Message()
        {
            
        }

       
    }
}
