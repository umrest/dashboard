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
        DashboardVisionData tag_1 = new DashboardVisionData("main");
        DashboardVisionData tag_2 = new DashboardVisionData("unloading");

        private ObservableCollection<DashboardVisionData> tags = new ObservableCollection<DashboardVisionData>();
        public VisionStateView()
        {
            InitializeComponent();

            tags.Add(tag_1);
            tags.Add(tag_2);

            vision_data_grid.ItemsSource = tags;
        }
    }
}
