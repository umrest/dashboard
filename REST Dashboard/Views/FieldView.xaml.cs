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
        // TAG 0 PARAMETERS

        // Width of the Sieve
        public static double T0_X_OFFSET = .3; // m
        // From center of sieve to corner of field
        public static double T0_Y_OFFSET = 1.5; // m


        public FieldView()
        {
            InitializeComponent();
            StateData.t0.PropertyChanged += Tag_PropertyChanged;
            StateData.t1.PropertyChanged += Tag_PropertyChanged;
        }


        private void Tag_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateRobotPosition();
        }

        public void SetRobotPosition(double field_x, double field_y, double yaw)
        {
            int x = (int)(field_x * 100);
            int y = (int)(field_y * 100);


            // Origin point is the left center of the dumping station
            double origin_x = (FieldCanvas.ActualHeight - Canvas.GetBottom(Sieve) - Sieve.Height / 2);
            double origin_y = Canvas.GetRight(Sieve) - Sieve.Width / 2;


            Canvas.SetTop(RobotRectangle, -RobotRectangle.Height / 2 + origin_x - x);
            Canvas.SetLeft(RobotRectangle, -RobotRectangle.Width + (origin_y - y));

            RotateTransform rotation = new RotateTransform(yaw, RobotRectangle.Width / 2, RobotRectangle.Height / 2);
            RobotRectangle.RenderTransform = rotation;
        }

        public void UpdateRobotPosition()
        {
            // Only using tag0 for now
            double x = StateData.t0.Z_ / 39.37; // in m
            double y = StateData.t0.X_ / 39.37; // in m

            double yaw = StateData.t0.yaw_; // in deg

            double field_x = T0_X_OFFSET + x;
            double field_y = T0_Y_OFFSET + y;

            //SetRobotPosition(field_x, field_y, yaw);
        }
    }
}
