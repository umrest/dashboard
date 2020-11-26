using REST_Dashboard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardNavigation_Path : comm.Navigation_Path, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public override void Deserialize(byte[] data)
        {
            base.Deserialize(data);
            PropertyChanged(this, new PropertyChangedEventArgs(null));
        }

        public DashboardNavigation_Path()
        {
            _point_0 = new DashboardNavigation_Point();
            _point_1 = new DashboardNavigation_Point();
            _point_2 = new DashboardNavigation_Point();
            _point_3 = new DashboardNavigation_Point();
            _point_4 = new DashboardNavigation_Point();
            _point_5 = new DashboardNavigation_Point();
            _point_6 = new DashboardNavigation_Point();
            _point_7 = new DashboardNavigation_Point();
            _point_8 = new DashboardNavigation_Point();
            _point_9 = new DashboardNavigation_Point();
            _point_10 = new DashboardNavigation_Point();
            _point_11 = new DashboardNavigation_Point();
            _point_12 = new DashboardNavigation_Point();
            _point_13 = new DashboardNavigation_Point();
            _point_14 = new DashboardNavigation_Point();
            _point_15 = new DashboardNavigation_Point();
            _point_16 = new DashboardNavigation_Point();
            _point_17 = new DashboardNavigation_Point();
            _point_18 = new DashboardNavigation_Point();

        }

        public DashboardNavigation_Point point_0
        {
            get
            {
                return (DashboardNavigation_Point)get_point_0();
            }
        }
        public DashboardNavigation_Point point_1
        {
            get
            {
                return (DashboardNavigation_Point)get_point_1();
            }
        }
        public DashboardNavigation_Point point_2
        {
            get
            {
                return (DashboardNavigation_Point)get_point_2();
            }
        }
        public DashboardNavigation_Point point_3
        {
            get
            {
                return (DashboardNavigation_Point)get_point_3();
            }
        }
        public DashboardNavigation_Point point_4
        {
            get
            {
                return (DashboardNavigation_Point)get_point_4();
            }
        }
        public DashboardNavigation_Point point_5
        {
            get
            {
                return (DashboardNavigation_Point)get_point_5();
            }
        }
        public DashboardNavigation_Point point_6
        {
            get
            {
                return (DashboardNavigation_Point)get_point_6();
            }
        }
        public DashboardNavigation_Point point_7
        {
            get
            {
                return (DashboardNavigation_Point)get_point_7();
            }
        }
        public DashboardNavigation_Point point_8
        {
            get
            {
                return (DashboardNavigation_Point)get_point_8();
            }
        }
        public DashboardNavigation_Point point_9
        {
            get
            {
                return (DashboardNavigation_Point)get_point_9();
            }
        }
        public DashboardNavigation_Point point_10
        {
            get
            {
                return (DashboardNavigation_Point)get_point_10();
            }
        }
        public DashboardNavigation_Point point_11
        {
            get
            {
                return (DashboardNavigation_Point)get_point_11();
            }
        }
        public DashboardNavigation_Point point_12
        {
            get
            {
                return (DashboardNavigation_Point)get_point_12();
            }
        }
        public DashboardNavigation_Point point_13
        {
            get
            {
                return (DashboardNavigation_Point)get_point_13();
            }
        }


    }


    public class DashboardNavigation_Point : comm.Navigation_Point, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public override void Deserialize(byte[] data)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(null));
            base.Deserialize(data);
        }
    }
}
