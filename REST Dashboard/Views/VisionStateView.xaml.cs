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
    public partial class VisionStateView : UserControl
    { 
        private ObservableCollection<DashboardTagPosition> tags = new ObservableCollection<DashboardTagPosition>();
        private ObservableCollection<DashboardFieldPosition> fps = new ObservableCollection<DashboardFieldPosition>();

        public VisionStateView()
        {
            InitializeComponent();

            tags.Add((DashboardTagPosition)(StateData.vision_data.t0));
            tags.Add((DashboardTagPosition)(StateData.vision_data.t1));

            fps.Add((DashboardFieldPosition)(StateData.vision_data.fp));
            vision_data_grid.ItemsSource = tags;
            vision_data_grid2.ItemsSource = fps;
        }
    }
}
