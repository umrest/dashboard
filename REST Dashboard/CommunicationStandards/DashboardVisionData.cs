using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    class DashboardVisionData : VisionData
    {
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
            throw new NotImplementedException();
        }
    }
}
