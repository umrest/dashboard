

namespace REST_Dashboard
{
    public struct MotorInfo
    {
        public float current { get; set; }
        public float voltage { get; set; }
        public float speed { get; set; }
        public float position { get; set; }
        public float percentage { get; set; }
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
            motor_info = new MotorInfo[8];
            gyro_data = new GyroData();
            gyro_data.yaw = 10;
            gyro_data.pitch = 11;
            gyro_data.roll = 12;

            motor_info[0].current = 12;

        }

        public new byte TYPE = 10;

        public MotorInfo[] motor_info;

        public GyroData gyro_data;

        public override byte[] Serialize()
        {
            throw new System.NotImplementedException();
        }

        public override void Deserialize(byte[] data)
        {
            throw new System.NotImplementedException();
        }
    }
}
