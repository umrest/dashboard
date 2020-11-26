using comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard.Handlers
{
    public class DashboardClient : RESTClient
    {
        public DashboardClient() : base("uofmrest.local", 8091, CommunicationDefinitions.IDENTIFIER.DASHBOARD)
        {
        }
        public override void on_connect()
        {
            StateData.mainwindow.update_indicators();
        }

        public override void on_disconnect()
        {
            StateData.mainwindow.update_indicators();
        }
    }
}
