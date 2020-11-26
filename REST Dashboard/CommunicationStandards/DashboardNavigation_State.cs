using REST_Dashboard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardNavigation_State : comm.Slam_State, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public override void Deserialize(byte[] data)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(null));
            base.Deserialize(data);
        }

        public DashboardNavigation_State()
        {
            _field_position = new DashboardFieldPosition("world");
        }

        public DashboardFieldPosition field_position
        {
            get
            {
                return (DashboardFieldPosition)get_field_position();
            }
        }
    }
}
