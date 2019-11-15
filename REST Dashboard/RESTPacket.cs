using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public abstract class RESTPacket
    {
        public byte TYPE = 0;

        public abstract byte[] Serialize();
        public abstract void Deserialize(byte[] data);

        protected const int TYPE_OFFSET = 0;
    }
}
