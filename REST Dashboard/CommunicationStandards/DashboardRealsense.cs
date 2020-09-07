using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardRealsense : comm.Realsense, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public DashboardRealsense()
        {
            _obstacle_1 = new DashboardObstacle("o1");
            _obstacle_2 = new DashboardObstacle("o2");
            _obstacle_3 = new DashboardObstacle("o3");
            _obstacle_4 = new DashboardObstacle("o4");
        }

        public DashboardObstacle[] obstacles
        {
            get
            {
                DashboardObstacle[] ret = new DashboardObstacle[4];
                ret[0] = (DashboardObstacle)get_obstacle_1();
                return ret;
            }
        }

    }

    public class DashboardObstacle : comm.Obstacle,  INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public DashboardObstacle(string label_in)
        {
            label = label_in;
        }
        
        public string label { get; set; }

        public double X {
            get {
                return get_x();
            }
        }
        public double Y {
            get
            {
                return get_y();

            }
        }
        public double width {
            get
            {
                return get_width();

            }
        }
        public double height {
            get
            {
                return get_height();
            }
        }
    }

}
