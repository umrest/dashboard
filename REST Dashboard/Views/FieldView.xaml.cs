using comm;
using SlimDX.XInput;
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
        private const int ROVER_WIDTH = 20;
        private const int ROVER_LENGTH = 30;


        private List<Ellipse> obstacles = new List<Ellipse>();

        private List<Line> lines = new List<Line>();



        public Point target_rover_position = new Point(0, 0);
        public double target_rover_angle = 0;

        Point rover_position = new Point(0, 0);

        private Point field_to_canvas(Point pt)
        {
            return new Point(pt.Y * 10, -pt.X * 10 + 500);
        }

        private Point canvas_to_field(Point pt)
        {
            return new Point(-(pt.Y - 500) / 10, pt.X / 10);
        }

        private void set_object_position(Shape shape, int x, int y, int width, int height, int angle, Point origin)
        {
            shape.Dispatcher.BeginInvoke((Action)(() =>
            {
                Point center_pt = field_to_canvas(new Point(x, y));

                shape.Height = width * 10;
                shape.Width = height * 10;

                TransformGroup transform = new TransformGroup();
                transform.Children.Add(new RotateTransform(angle));
                transform.Children.Add(new TranslateTransform(center_pt.X, center_pt.Y));


                shape.RenderTransformOrigin = origin;
                shape.RenderTransform = transform;



            }
            ));

        }

        private void _set_rover_position(Shape shape, Point pos, int angle)
        {
            Point pos2 = new Point(pos.X + ROVER_WIDTH / 2.0, pos.Y);

            set_object_position(shape, (int)pos2.X, (int)pos2.Y, ROVER_WIDTH, ROVER_LENGTH, -angle, new Point(0.5, 0.5));
        }

        private void set_rover_position(Point pos, int angle)
        {
            _set_rover_position(rover, pos, angle);
        }
        public void set_target_rover_position()
        {

            _set_rover_position(target_rover, target_rover_position, (int)target_rover_angle);
        }

        private void set_obstacle_position(int index, int x, int y, int radius)
        {
            set_object_position(obstacles[index], x, y, radius, radius, 0, new Point(0.5, 0.5));
        }


        public FieldView()
        {
            InitializeComponent();

            StateData.navigation_state.field_position.PropertyChanged += field_position_PropertyChanged;
            StateData.navigation_obstacles.PropertyChanged += obstacles_PropertyChanged;
            StateData.navigation_path.PropertyChanged += path_PropertyChanged;

            obstacles.Add(new Ellipse());
            obstacles.Add(new Ellipse());
            obstacles.Add(new Ellipse());
            obstacles.Add(new Ellipse());

            FieldCanvas.Children.Add(obstacles[0]);
            FieldCanvas.Children.Add(obstacles[1]);
            FieldCanvas.Children.Add(obstacles[2]);
            FieldCanvas.Children.Add(obstacles[3]);

            set_rover_position(rover_position, 0);
            set_target_rover_position();
            set_obstacle_position(0, 0, 0, 0);
            set_obstacle_position(1, 0, 0, 0);
            set_obstacle_position(2, 0, 0, 0);
            set_obstacle_position(3, 0, 0, 0);

            rover.Fill = new SolidColorBrush(Colors.DarkBlue);
            target_rover.Fill = new SolidColorBrush(Colors.LightBlue);
            var obstacle_color = new SolidColorBrush(Colors.DarkRed);

            obstacles[0].Fill = obstacle_color;
            obstacles[1].Fill = obstacle_color;
            obstacles[2].Fill = obstacle_color;
            obstacles[3].Fill = obstacle_color;

            for (int i = 0; i < 20; i++)
            {
                var line = new Line();
                lines.Add(line);
                FieldCanvas.Children.Add(line);
            }


        }

        private void path_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            List<Navigation_Point> points = new List<Navigation_Point>();

            points.Add(StateData.navigation_path.point_0);
            points.Add(StateData.navigation_path.point_1);
            points.Add(StateData.navigation_path.point_2);
            points.Add(StateData.navigation_path.point_3);
            points.Add(StateData.navigation_path.point_4);
            /*points.Add(StateData.navigation_path.point_5);
            points.Add(StateData.navigation_path.point_6);
            points.Add(StateData.navigation_path.point_7);
            points.Add(StateData.navigation_path.point_8);
            points.Add(StateData.navigation_path.point_9);
            points.Add(StateData.navigation_path.point_10);
            points.Add(StateData.navigation_path.point_11);
            points.Add(StateData.navigation_path.point_12);
            points.Add(StateData.navigation_path.point_13);*/

            FieldCanvas.Dispatcher.BeginInvoke((Action)(() =>
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    var line = lines[i];
                    line.StrokeThickness = 1;
                    line.Stroke = new SolidColorBrush(Colors.Green);

                    Point canvas_pt1 = field_to_canvas(new Point(points[i].get_x(), points[i].get_y()));
                    line.X1 = canvas_pt1.X;
                    line.Y1 = canvas_pt1.Y;

                    Point canvas_pt2 = field_to_canvas(new Point(points[i + 1].get_x(), points[i + 1].get_y()));

                    line.X2 = canvas_pt2.X;
                    line.Y2 = canvas_pt2.Y;


                }

            }
            ));


        }

        private void obstacles_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            set_obstacle_position(0, (StateData.navigation_obstacles.get_obstacle_0().get_x()), (StateData.navigation_obstacles.get_obstacle_0().get_y()), (StateData.navigation_obstacles.get_obstacle_0().get_width()));
            set_obstacle_position(1, StateData.navigation_obstacles.get_obstacle_1().get_x(), (StateData.navigation_obstacles.get_obstacle_1().get_y()), (StateData.navigation_obstacles.get_obstacle_1().get_width()));
            set_obstacle_position(2, (StateData.navigation_obstacles.get_obstacle_2().get_x()), (StateData.navigation_obstacles.get_obstacle_2().get_y()), (StateData.navigation_obstacles.get_obstacle_2().get_width()));
            set_obstacle_position(3, (StateData.navigation_obstacles.get_obstacle_3().get_x()), (StateData.navigation_obstacles.get_obstacle_3().get_y()), (StateData.navigation_obstacles.get_obstacle_3().get_width()));

        }

        private void field_position_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            rover_position.X = StateData.navigation_state.get_field_position().get_x();
            rover_position.Y = StateData.navigation_state.get_field_position().get_y();

            set_rover_position(rover_position, (int)(StateData.navigation_state.get_field_position().get_yaw()));
        }

        bool selecting_target_position = false;


        private void FieldCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selecting_target_position = true;

        }

        private double limit_range(double kx, double kUpperBound, double kLowerBound)
        {
            double range = kUpperBound - kLowerBound;
            kx = ((kx - kLowerBound) % range) + range;
            return (kx % range) + kLowerBound;
        }

        private double fix_angle(double angle)
        {
            return limit_range(angle, 180, -180);
        }

        private void FieldCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (selecting_target_position)
            {
                target_rover_angle += (e.Delta / 24.0);

                target_rover_angle = fix_angle(target_rover_angle);
                set_target_rover_position();
            }
        }

        private void FieldCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (selecting_target_position)
            {
                target_rover_position = canvas_to_field(e.GetPosition(FieldCanvas));

                set_target_rover_position();
            }
        }

        private void FieldCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selecting_target_position = false;
        }
    }
}
