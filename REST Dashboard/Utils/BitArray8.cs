using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    class BitArray8
    {
        public byte aByte = 0;
       public BitArray8()
        {
            
        }

        public void SetBit(int pos, bool value)
        {
            if (value)
            {
                //left-shift 1, then bitwise OR
                aByte = (byte)(aByte | (1 << pos));
            }
            else
            {
                //left-shift 1, then take complement, then bitwise AND
                aByte = (byte)(aByte & ~(1 << pos));
            }
        }

        public bool GetBit(int pos)
        {
            //left-shift 1, then bitwise AND, then check for non-zero
            return ((aByte & (1 << pos)) != 0);
        }
    }
}
