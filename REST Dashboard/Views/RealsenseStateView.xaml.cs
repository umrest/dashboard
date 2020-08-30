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
    public partial class RealsenseStateView : UserControl
    { 
        private ObservableCollection<DashboardObstacle> tags = new ObservableCollection<DashboardObstacle>();

        public RealsenseStateView()
        {
            InitializeComponent();

            tags.Add((DashboardObstacle)(StateData.realsense_data.o1));
            tags.Add((DashboardObstacle)(StateData.realsense_data.o2));


            tags.Add((DashboardObstacle)(StateData.realsense_data.o3));
            tags.Add((DashboardObstacle)(StateData.realsense_data.o4));

            data_grid.ItemsSource = tags;
            
        }
    }
}
