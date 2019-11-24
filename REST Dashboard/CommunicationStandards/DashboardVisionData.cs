using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardVisionData : VisionData
    {
        
        

        public DashboardVisionData() : base()
        {
            t1 = new DashboardTagPosition("main");
            t2 = new DashboardTagPosition("unloading");
        }
        public override byte[] Serialize()
        {
            throw new NotFiniteNumberException();
        }

        public override void Deserialize(byte[] data)
        {
            byte[] t1_data = new byte[12];
            Array.Copy(data, TAG_1_OFFSET, t1_data, 0, 12);
            t1.Deserialize(t1_data);

            byte[] t2_data = new byte[12];
            Array.Copy(data, TAG_2_OFFSET, t2_data, 0, 12);
            t2.Deserialize(t2_data);           
        }
    }
    public class DashboardTagPosition : TagPosition,  INotifyPropertyChanged
    {
        public DashboardTagPosition(string label_in)
        {
            label = label_in;
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public override void Deserialize(byte[] data)
        {
            X = BitConverter.ToInt16(data, X_OFFSET);
            Y = BitConverter.ToInt16(data, Y_OFFSET);
            Z = BitConverter.ToInt16(data, Z_OFFSET);
            
            yaw = BitConverter.ToInt16(data, YAW_OFFSET);
            pitch = BitConverter.ToInt16(data, PITCH_OFFSET);
            roll = BitConverter.ToInt16(data, ROLL_OFFSET);

            PropertyChanged(this, new PropertyChangedEventArgs(null));

        }

        public string label { get; set; }
        public double scale(short s)
        {
            return s / 364.0888;
        }
        public double scale_xyz(short s)
        {
            return s / 10.0;
        }
        public double yaw_
        {
            get
            {
                return scale(yaw);
            }
        }
        public double pitch_
        {
            get
            {
                return scale(pitch);
            }
        }
        public double roll_
        {
            get
            {
                return scale(roll);
            }
        }
        public double X_
        {
            get
            {
                return scale_xyz(X);
            }
        }
        public double Y_
        {
            get
            {
                return scale_xyz(Y);
            }
        }
        public double Z_
        {
            get
            {
                return scale_xyz(Z);
            }
        }
    }
}
