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
    public class DashboardVision : comm.Vision
    {
        public DashboardVision() : base()
        {
            _field_position = new DashboardFieldPosition("fp");
            _tag0 = new DashboardTagPosition("tag0");
            _tag1 = new DashboardTagPosition("tag1");
        }

        public DashboardFieldPosition field_position
        {
            get
            {
                return (DashboardFieldPosition)get_field_position();
            }
        }

        public DashboardTagPosition tag0
        {
            get
            {
                return (DashboardTagPosition)get_tag0();
            }
        }

        public DashboardTagPosition tag1
        {
            get
            {
                return (DashboardTagPosition)get_tag1();
            }
        }
    }

    public class DashboardTagPosition : comm.Tag_Position,  INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public DashboardTagPosition(string label_in)
        {
            label = label_in;
        }
     
        public string label { get; set; }
        
        
        public double yaw
        {
            get
            {
                return get_yaw();
            }
        }
        public double pitch
        {
            get
            {
                return get_pitch();
            }
        }
        public double roll
        {
            get
            {
                return get_roll();
            }
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
        public double Z
        {
            get
            {
                return get_z();
            }
        }
    }

    public class DashboardFieldPosition : comm.Field_Position, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public DashboardFieldPosition(string label_in)
        {
            label = label_in;
        }

        public string label { get; set; }

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

        public double yaw
        {
            get
            {
                return get_yaw();
            }
        }




    }
}
