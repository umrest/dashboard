using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for VisionView.xaml
    /// </summary>
    public partial class VisionView : UserControl
    {
        public VisionView()
        {
            InitializeComponent();
        }

        public void SetImage(byte[] image)
        {
            try
            {
                Stream imageStreamSource = new MemoryStream(image);

                JpegBitmapDecoder decoder = new JpegBitmapDecoder(imageStreamSource, BitmapCreateOptions.None, BitmapCacheOption.None);

                BitmapSource bitmapSource = decoder.Frames[0];

                vision_image.Dispatcher.BeginInvoke((Action)(() => vision_image.Source = bitmapSource));
            }
            catch
            {
                //MessageBox.Show("Invalid Image...");
            }
            
             
            
        }
    }
}
