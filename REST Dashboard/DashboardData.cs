using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardData
    {

        public DashboardData()
        {

        }

        public bool enabled = false;
        public bool estop = false;


        public byte[] Serialize()
        {
            byte[] ret = new byte[128];
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    byte type = 9;

                    writer.Write(type);

                    BitArray8 critical_state = new BitArray8();

                    critical_state.SetBit(0, enabled);
                    critical_state.SetBit(1, estop);

                    

                    writer.Write(critical_state.aByte);
                    writer.Write(critical_state.aByte);
                }

                m.ToArray().CopyTo(ret, 0);
                return ret;
            }
        }

        public void Deserialize(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    byte type;

                    type = reader.ReadByte();


                    BitArray8 critical_state = new BitArray8();

                    critical_state.aByte = reader.ReadByte();

                    enabled = critical_state.GetBit(0);
                    estop = critical_state.GetBit(1);

                }
            }
        }
    }
}
