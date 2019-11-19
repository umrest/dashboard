using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardVisionData : VisionData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private short _X;
        public new short X {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
                PropertyChanged(this, new PropertyChangedEventArgs("X"));
            }
        }

        private short _Y;
        public new short Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Y"));
            }
        }

        private short _Z;
        public new short Z
        {
            get
            {
                return _Z;
            }
            set
            {
                _Z = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Z"));
            }
        }

        private short _yaw;
        public new short yaw
        {
            get
            {
                return _yaw;
            }
            set
            {
                _yaw = value;
                PropertyChanged(this, new PropertyChangedEventArgs("yaw"));
            }
        }

        private short _pitch;
        public new short pitch
        {
            get
            {
                return _pitch;
            }
            set
            {
                _pitch = value;
                PropertyChanged(this, new PropertyChangedEventArgs("pitch"));
            }
        }

        private short _roll;
        public new short roll
        {
            get
            {
                return _roll;
            }
            set
            {
                _roll = value;
                PropertyChanged(this, new PropertyChangedEventArgs("roll"));
            }
        }
        public string label { get; set; }

        public DashboardVisionData(string label_in) : base()
        {
            label = label_in;
        }
        public override byte[] Serialize()
        {
            throw new NotFiniteNumberException();
        }

        public override void Deserialize(byte[] data)
        {
            X = BitConverter.ToInt16(data, X_OFFSET);
            Y = BitConverter.ToInt16(data, Y_OFFSET);
            Z = BitConverter.ToInt16(data, Z_OFFSET);

            yaw = BitConverter.ToInt16(data, YAW_OFFSET);
            pitch = BitConverter.ToInt16(data, PITCH_OFFSET);
            roll = BitConverter.ToInt16(data, ROLL_OFFSET);
        }
    }
}
