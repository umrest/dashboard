using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    class DashboardDataAggregatorState :  DataAggregatorState
    {
        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(byte[] data)
        {
            BitArray8 status = new BitArray8();
            status.aByte = data[1];

            hero_connected = status.GetBit(0);
        }
    }
}
