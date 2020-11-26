using REST_Dashboard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardNavigation_Obstacles : comm.Navigation_Obstacles, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public override void Deserialize(byte[] data)
        {
            base.Deserialize(data);
            PropertyChanged(this, new PropertyChangedEventArgs(null));
        }

        public DashboardNavigation_Obstacles()
        {
            _obstacle_0 = new DashboardNavigation_Obstacle();
            _obstacle_1 = new DashboardNavigation_Obstacle();
            _obstacle_2 = new DashboardNavigation_Obstacle();
            _obstacle_3 = new DashboardNavigation_Obstacle();
        }

        public DashboardNavigation_Obstacle[] obstacles
        {
            get
            {
                DashboardNavigation_Obstacle[] ret = new DashboardNavigation_Obstacle[4];
                ret[0] = (DashboardNavigation_Obstacle)get_obstacle_0();
                ret[1] = (DashboardNavigation_Obstacle)get_obstacle_1();
                ret[2] = (DashboardNavigation_Obstacle)get_obstacle_2();
                ret[3] = (DashboardNavigation_Obstacle)get_obstacle_3();
                return ret;
            }
        }

       
    }

    public class DashboardNavigation_Obstacle : comm.Navigation_Obstacle, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public override void Deserialize(byte[] data)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(null));
            base.Deserialize(data);
        }

        public double X
        {
            get
            {
                return get_x();
            }
        }
        public double Y
        {
            get
            {
                return get_y();

            }
        }
        public double width
        {
            get
            {
                return get_width();

            }
        }
        public double height
        {
            get
            {
                return get_height();
            }
        }
    }
}
