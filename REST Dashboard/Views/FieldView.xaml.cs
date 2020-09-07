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
            Canvas.SetTop(rover, (100-x));
            Canvas.SetLeft(rover, y);
        }

        public FieldView()
        {
            InitializeComponent();

            rover.Width = 20;
            rover.Height = 20;

            set_rover_position(5, 5);



            rover.Fill = new SolidColorBrush(System.Windows.Media.Colors.DarkBlue);


            FieldCanvas.Children.Add(rover);
            FieldCanvas.Children.Add(obstacle_0);
        }


    }
}
