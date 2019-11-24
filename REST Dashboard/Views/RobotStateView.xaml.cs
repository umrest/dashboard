using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace REST_Dashboard
{
    /// <summary>
    /// Interaction logic for VisionStateView.xaml
    /// </summary>
    public partial class RobotStateView : UserControl
    {

        RobotStateData robot_state = new RobotStateData();

        public RobotStateView()
        {
            InitializeComponent();

            ObservableCollection<GyroData> gyro_data = new ObservableCollection<GyroData>();
            gyro_data.Add(robot_state.gyro_data);

            gyro_data_grid.ItemsSource = gyro_data;

            ObservableCollection<MotorInfo> motor_info = new ObservableCollection<MotorInfo>(robot_state.motor_info);

            motor_data_grid.ItemsSource = motor_info;
        }
    }
}
