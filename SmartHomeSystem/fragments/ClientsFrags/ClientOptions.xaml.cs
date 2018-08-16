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

namespace SmartHomeSystem.fragments.ClientsFrags
{
    /// <summary>
    /// Interaction logic for ClientOptions.xaml
    /// </summary>
    public partial class ClientOptions : Window
    {

        SolidColorBrush selectedBrush = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(156), Convert.ToByte(208), Convert.ToByte(49)));
        SolidColorBrush backgroundBrushselectedBrush = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(230), Convert.ToByte(231), Convert.ToByte(237)));
        Guid selectedOptionGuid = new Guid();

        public ClientOptions()
        {
            InitializeComponent();
            ClientOptionsFrags.ClientOptionsOptionxaml ClientOptionsOption = new ClientOptionsFrags.ClientOptionsOptionxaml();
            ClientOptionsOption.DetachContent();
            ClientFragmentManager.Children.Add(ClientOptionsOption.ClientOptionsOptionn);
        }

        public ClientOptions(Guid guid)
        {
            InitializeComponent();
            //ClientOptionsFrags.ClientOptionsOptionxaml ClientOptionsOption = new ClientOptionsFrags.ClientOptionsOptionxaml(guid);
            //ClientOptionsOption.DetachContent();
            //ClientFragmentManager.Children.Add(ClientOptionsOption.ClientOptionsOptionn);

            //selectedOptionGuid = guid;
        }

        void setSelectedToNon()
        {
            lblOptions.Background = backgroundBrushselectedBrush;
            lblProducts.Background = backgroundBrushselectedBrush;
        }

        private void lblOptions_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //setSelectedToNon();
            //lblOptions.Background = selectedBrush;
            //ClientFragmentManager.Children.RemoveAt(0);
            //ClientOptionsFrags.ClientOptionsOptionxaml ClientOptionsOption = new ClientOptionsFrags.ClientOptionsOptionxaml(selectedOptionGuid);
            //ClientOptionsOption.DetachContent();
            //ClientFragmentManager.Children.Add(ClientOptionsOption.ClientOptionsOptionn);
        }

        private void lblProducts_MouseDown(object sender, MouseButtonEventArgs e)
        {
            setSelectedToNon();
            lblProducts.Background = selectedBrush;
            ClientFragmentManager.Children.RemoveAt(0);
            ClientOptionsFrags.ClientOptionsProduct ClientOpitonsProduct = new ClientOptionsFrags.ClientOptionsProduct(selectedOptionGuid);
            ClientOpitonsProduct.DetachContent();
            ClientFragmentManager.Children.Add(ClientOpitonsProduct.ClientOptionsProductt);
        }
    }
}
