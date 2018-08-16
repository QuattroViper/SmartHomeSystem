using ClassLibrary.classes;
using ClassLibrary.classes.lazyLoad.client;
using ErrorHandler;
using SmartHomeSystem.fragments.ClientsFrags;
using SmartHomeSystem.fragments.ClientsFrags.ClientOptionsFrags;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
using EventBus;

namespace SmartHomeSystem.fragments.productFrags
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window, IDetachContent, IEventBus //, INotifyProductChange
    {

        List<OptionLazy> options = new List<OptionLazy>();
        OptionLazy selectedOptions = null;

        public ProductWindow()
        {
            InitializeComponent();
            updateView();

            EventBus.EventBus.Instance.Register(this);

            lblNewOption.IsEnabled = (Global.ADUser.IsAdministrator == true) ? true : false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            EventBus.EventBus.Instance.Unregister(this);
        }

        public void DetachContent()
        {
            RemoveLogicalChild(Content);
        }

        public void updateView()
        {
            OptionLazy optionLazy = new OptionLazy();

            options = optionLazy.LoadAllOptions();

            lblProductCount.Content = options.Count < 10 ? string.Format("0{0}",options.Count) : options.Count.ToString() ;

            lvOptions.ItemsSource = options;

        }

        private void lvAllOptions_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {

                try
                {
                    //dynamic selectedClient = (ExpandoObject)item;

                    selectedOptions = (OptionLazy)item;
                    btnAllProducts.IsEnabled = true;

                    lvFunctions.ItemsSource = selectedOptions.Functions;


                    btnUpdateOption.IsEnabled = (Global.ADUser.IsAdministrator == true) ? true : false;
                    btnDeleteProduct.IsEnabled = (Global.ADUser.IsAdministrator == true) ? true : false;

                    // Needs to be moved to Products
                    //ClientOptions CO = new ClientOptions(selectedOptions.GUID);
                    //CO.Show();


                }
                catch (Exception exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                        error.handle(exception, true, true);
                }


            }
        }

        private void btnAllProducts_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateToWithoutHide(new ClientOptionsProduct(selectedOptions.GUID));
        }

        private void lblNewOption_MouseUp(object sender, MouseButtonEventArgs e)
        {

            NavigationService.NavigateToWithoutHide(new ClientOptionsOptionxaml());
        }

        public void OnEvent(CustomEvent customEvent)
        {
            if (customEvent.EventString.Equals("OptionInserted"))
            {
                updateView();
            }
        }

        private void btnUpdateOption_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateToWithoutHide(new ClientOptionsOptionxaml(selectedOptions));
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {

            if (selectedOptions != null)
            {

                selectedOptions.deleteOption();

                EventBus.EventBus.Instance.PostEvent(new CustomEvent("OptionInserted"));
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option successfully deleted", CustomEvent.EventType.accept));
            }
            

            
        }
    }
}
