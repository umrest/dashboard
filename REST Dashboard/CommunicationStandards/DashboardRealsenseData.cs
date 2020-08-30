using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardRealsenseData : RealsenseData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public DashboardRealsenseData() : base()
        {
            o1 = new DashboardObstacle("o1");
            o2 = new DashboardObstacle("o2");
            o3 = new DashboardObstacle("o3");
            o4 = new DashboardObstacle("o4");
        }
        public override byte[] Serialize()
        {
            throw new NotFiniteNumberException();
        }

        public override void Deserialize(byte[] data)
        {
            byte[] o1_data = new byte[12];
            Array.Copy(data, OBSTACLE_1_OFFSET, o1_data, 0, 12);
            o1.Deserialize(o1_data);

            byte[] o2_data = new byte[12];
            Array.Copy(data, OBSTACLE_2_OFFSET, o2_data, 0, 12);
            o2.Deserialize(o2_data);

            byte[] o3_data = new byte[12];
            Array.Copy(data, OBSTACLE_3_OFFSET, o3_data, 0, 12);
            o3.Deserialize(o3_data);

            byte[] o4_data = new byte[12];
            Array.Copy(data, OBSTACLE_4_OFFSET, o4_data, 0, 12);
            o4.Deserialize(o4_data);
            PropertyChanged(this, new PropertyChangedEventArgs(null));
        }
    }

    public class DashboardObstacle : Obstacle,  INotifyPropertyChanged
    {
        public DashboardObstacle(string label_in)
        {
            label = label_in;
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public override void Deserialize(byte[] data)
        {
            X = BitConverter.ToInt16(data, X_OFFSET);
            Y = BitConverter.ToInt16(data, Y_OFFSET);
            width = BitConverter.ToInt16(data, WIDTH_OFFSET);
            height = BitConverter.ToInt16(data, HEIGHT_OFFSET);

            PropertyChanged(this, new PropertyChangedEventArgs(null));

        }

        public string label { get; set; }

        public double _X {
            get {
                return X / 10.0;

            }
        }
        public double _Y {
            get
            {
                return Y / 10.0;

            }
        }
        public double _width {
            get
            {
                return width / 10.0;

            }
        }
        public double _height {
            get
            {
                return height / 10.0;

            }  }
    }

}
