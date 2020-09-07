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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace REST_Dashboard.Views
{
    /// <summary>
    /// Interaction logic for FieldView.xaml
    /// </summary>
    public partial class FieldView : UserControl
    {
        private Rectangle rover = new Rectangle();
        private Ellipse obstacle_0 = new Ellipse();

        private void set_rover_position(int x, int y)
        {
            rover.Dispatcher.BeginInvoke((Action)(() =>
            {
            Canvas.SetTop(rover, 50 - rover.Height/2 + x);
                Canvas.SetLeft(rover, y);
            }
            ));
 
        }

        private void set_obstacle_position(int x, int y, int radius)
        {
            obstacle_0.Dispatcher.BeginInvoke((Action)(() =>
            {
                obstacle_0.Height = radius;
                obstacle_0.Width = radius;

                Canvas.SetTop(obstacle_0, 50 - obstacle_0.Height / 2 + x);
                Canvas.SetLeft(obstacle_0, y);
            }
            ));

        }

        public FieldView()
        {
            InitializeComponent();

            StateData.navigation_state.field_position.PropertyChanged += field_position_PropertyChanged;
            StateData.navigation_obstacles.obstacle_0.PropertyChanged += obstacle_0_PropertyChanged; ;

            rover.Width = 20;
            rover.Height = 20;

            set_rover_position(0, 0);
            set_obstacle_position(5, 0, 5);

            rover.Fill = new SolidColorBrush(System.Windows.Media.Colors.DarkBlue);
            obstacle_0.Fill = new SolidColorBrush(System.Windows.Media.Colors.DarkRed);

            FieldCanvas.Children.Add(rover);
            FieldCanvas.Children.Add(obstacle_0);
        }

        private void obstacle_0_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            set_obstacle_position((int)(StateData.navigation_obstacles.get_obstacle_0().get_x()), (int)(StateData.navigation_obstacles.get_obstacle_0().get_y()), (int)(int)(StateData.navigation_obstacles.get_obstacle_0().get_width()));
        }

        private void field_position_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            set_rover_position((int)(StateData.navigation_state.get_field_position().get_x()), (int)(StateData.navigation_state.get_field_position().get_y()));
        }
    }
}
