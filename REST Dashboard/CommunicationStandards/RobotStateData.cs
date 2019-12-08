

namespace REST_Dashboard
{
    public class MotorInfo
    {
        public int can_id { get; set; }
        public double current { get; set; }
        public double voltage { get; set; }
        public double speed { get; set; }
        public double position { get; set; }
        public double percentage { get; set; }

        public static int MOTOR_INFO_SIZE = 15;
        public static int MOTOR_INFO_CAN_OFFSET = 0;
        public static int MOTOR_INFO_CURRENT_OFFSET = 1;
        public static int MOTOR_INFO_POSITION_OFFSET = MOTOR_INFO_CURRENT_OFFSET + 2;
        public static int MOTOR_INFO_VELOCITY_OFFSET = MOTOR_INFO_POSITION_OFFSET + 8;
    }

    public struct GyroData
    {
        public float yaw { get; set; }
        public float pitch { get; set; } 
        public float roll { get; set; } 
    }
    public class RobotStateData : RESTPacket
    {
        public RobotStateData()
        {

        }

        public new byte TYPE = 10;



        public override byte[] Serialize()
        {
            throw new System.NotImplementedException();
        }

        public override void Deserialize(byte[] data)
        {
            throw new System.NotImplementedException();
        }

        public static int MOTOR_INFO_OFFSET = 1;

    }
}
