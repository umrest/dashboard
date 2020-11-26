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
    public partial class NavigationStateView : UserControl
    {
        private ObservableCollection<DashboardNavigation_Obstacle> obstacles = new ObservableCollection<DashboardNavigation_Obstacle>();
        private ObservableCollection<DashboardFieldPosition> fp = new ObservableCollection<DashboardFieldPosition>();

        public NavigationStateView()
        {
            InitializeComponent();

            obstacles = new ObservableCollection<DashboardNavigation_Obstacle>(StateData.navigation_obstacles.obstacles);
            fp.Add(StateData.navigation_state.field_position);

            data_grid.ItemsSource = obstacles;
            data_grid2.ItemsSource = fp;

        }
    }
}
