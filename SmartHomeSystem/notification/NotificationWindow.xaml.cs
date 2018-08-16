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
using System.Windows.Shapes;

namespace SmartHomeSystem.notification
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window, IDetachContent
    {
        public NotificationWindow()
        {
            InitializeComponent();
        }

        public NotificationWindow(string message, SolidColorBrush backgroundColor)
        {
            InitializeComponent();
        }

        public void DetachContent()
        {
            RemoveLogicalChild(Content);
        }
    }
}
