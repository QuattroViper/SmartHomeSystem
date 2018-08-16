using ClassLibrary.classes;
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

namespace SmartHomeSystem.client
{
    /// <summary>
    /// Interaction logic for SystemLoginsWindow.xaml
    /// </summary>
    public partial class SystemLoginsWindow : Window
    {
        public SystemLoginsWindow()
        {
            InitializeComponent();

            SystemLogins login = new SystemLogins();
            lvSystemLogins.ItemsSource = login.getAllLogings();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.NavigateBack();
        }
    }
}
