using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for LogView.xaml
    /// </summary>
    ///
    public partial class LogView : UserControl
    {
        public class LogMessage
        {
            public string message = "NO MESSAGE";
            public string header = "NO HEADER";
        }

        public ObservableCollection<LogMessage> log_messages  { get; private set; } = new ObservableCollection<LogMessage>();

        public LogView()
        {
            DataContext = this;
           
            InitializeComponent();
            var mes = new LogMessage();
            log_messages.Add(mes);
        }

        public void add_message(string header, string message)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                var mes = new LogMessage();
                mes.message = message;
                mes.header = header;
                log_messages.Add(mes);
            }));
        }
    }
}
