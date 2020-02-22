using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace REST_Dashboard
{
    /// <summary>
    /// Interaction logic for FieldView.xaml
    /// </summary>
    public partial class FieldView : UserControl
    {
        // TAG 1 PARAMETERS
        public static double FIELD_SIZE_Y = 3.6;
        public static double FIELD_SIZE_X = 5.4;

        // Width of the Sieve
        public static double T1_Y_OFFSET = .3; // m
        // From center of sieve to corner of field
        public static double T1_X_OFFSET = 1.5; // m

        public static double CAMERA_X_OFFSET = 0;
        public static double CAMERA_Y_OFFSET = .4;

     


        public FieldView()
        {
            InitializeComponent();
            StateData.fp.PropertyChanged += Tag_PropertyChanged;
        }


        private void Tag_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateRobotPosition();
        }
        private void SetRobotPositionValid(bool valid)
        {
            RobotRectangle.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (valid)
                {
                    RobotRectangle.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                }
                else
                {

                    RobotRectangle.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));

                }
            }
            )
            );
        }
        private void SetRobotPosition(double field_x, double field_y, double yaw)
        {
            // Origin point is the bottom right corner
            double origin_x = FieldCanvas.ActualWidth - T1_X_OFFSET * 100;
            double origin_y = FieldCanvas.ActualHeight - T1_Y_OFFSET * 100;

            double x = origin_x - field_x * 100; // convert to pixels
            double y = origin_y - field_y * 100;

            RobotRectangle.Dispatcher.BeginInvoke((Action)(() =>
            {
                double left = x - RobotRectangle.ActualWidth / 2;
                double top = y - RobotRectangle.ActualHeight / 2 - CAMERA_Y_OFFSET * 100;


                Canvas.SetTop(RobotRectangle, top);
                Canvas.SetLeft(RobotRectangle, left);

                RotateTransform rotation = new RotateTransform(yaw + 90, RobotRectangle.ActualWidth / 2, RobotRectangle.ActualHeight / 2);
                RobotRectangle.RenderTransform = rotation;
            }
            ));
        }

        public void UpdateRobotPosition()
        {

            double field_x = StateData.fp.X_ / 39.3701;

            double field_y = StateData.fp.Y_ / 39.3701;

            double yaw = StateData.fp.yaw_;

            if(StateData.vision_data.vision_good == 1)
            {
                SetRobotPositionValid(true);

                SetRobotPosition(field_x, field_y, yaw);
            }

            else
            {
                SetRobotPositionValid(false);
            }

            
        }
    }
}
