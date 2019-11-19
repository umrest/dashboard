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
        public FieldView()
        {
            InitializeComponent();
        }

        public void SetRobotPosition(int x, int y, int yaw)
        {

            /*
            TranslateTransform trans = new TranslateTransform();
            RobotRectangle.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(top, y, TimeSpan.FromSeconds(1));
            DoubleAnimation anim2 = new DoubleAnimation(left, x, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, anim1);
            trans.BeginAnimation(TranslateTransform.YProperty, anim2);

    */
            // Origin point is the left center of the dumping station
            double origin_x = (Canvas.GetTop(DumpingStation) + DumpingStation.Height / 2);
            double origin_y = Canvas.GetLeft(DumpingStation);
            Canvas.SetTop(RobotRectangle, -RobotRectangle.Height/2 + origin_x - x * 10);
            Canvas.SetLeft(RobotRectangle, -RobotRectangle.Width + (origin_y - y * 10));

            RotateTransform rotation = new RotateTransform(yaw, RobotRectangle.Width/2,RobotRectangle.Height/2);
            RobotRectangle.RenderTransform = rotation;
        }
    }
}
