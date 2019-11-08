using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class JoystickData
    {
       
        public JoystickData()
        {

        }

        public bool button_a;
        public bool button_b;
        public bool button_x;
        public bool button_y;

        public bool button_rb;
        public bool button_lb;
        public bool button_lj;
        public bool button_rj;

        public float lj_x;
        public float lj_y;
        public float rj_x;
        public float rj_y;
        public float lt;
        public float rt;

        public byte[] Serialize()
        {
            byte[] ret = new byte[128];
            using(MemoryStream m = new MemoryStream())
            {
                using(BinaryWriter writer = new BinaryWriter(m))
                {
                    byte type = 1;

                    BitArray8 button_data_1 =  new BitArray8();
                    BitArray8 button_data_2 = new BitArray8();

                    button_data_1.SetBit(0, button_a);
                    button_data_1.SetBit(1, button_b);
                    button_data_1.SetBit(2, button_x);
                    button_data_1.SetBit(3, button_y);
                    button_data_1.SetBit(4, button_lb);
                    button_data_1.SetBit(5, button_rb);
                    button_data_1.SetBit(6, button_lj);
                    button_data_1.SetBit(7, button_rj);
                                       
                    writer.Write(type);

                    writer.Write(button_data_1.aByte);
                    writer.Write(button_data_2.aByte);
                    writer.Write(lj_x);
                    writer.Write(lj_y);
                    writer.Write(rj_x);
                    writer.Write(rj_y);
                    writer.Write(lt);
                    writer.Write(rt);
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

                    

                    BitArray8 button_data_1 = new BitArray8();
                    BitArray8 button_data_2 = new BitArray8();

                    button_data_1.aByte = reader.ReadByte();
                    button_data_2.aByte = reader.ReadByte();



                    button_a = button_data_1.GetBit(0);
                    button_b = button_data_1.GetBit(1);
                    button_x = button_data_1.GetBit(2);
                    button_y = button_data_1.GetBit(3);
                    button_lb = button_data_1.GetBit(4);
                    button_rb = button_data_1.GetBit(5);
                    button_lj = button_data_1.GetBit(6);
                    button_rj = button_data_1.GetBit(7);


                    lj_x = reader.ReadSingle();
                    lj_y = reader.ReadSingle();
                    rj_x = reader.ReadSingle();
                    rj_y = reader.ReadSingle();
                    lt = reader.ReadSingle();
                    rt = reader.ReadSingle();
                }
            }
        }
    }
}
