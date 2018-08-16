using ClassLibrary.classes;
using ClassLibrary.classes.Combined;
using ClassLibrary.classes.lazyLoad.client;
using ErrorHandler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SmartHomeSystem.fragments.ClientsFrags.ClientOptionsFrags
{
    /// <summary>
    /// Interaction logic for ClientOptionsProduct.xaml
    /// </summary>
    public partial class ClientOptionsProduct : Window, IDetachContent, INotifyClientChange, IEventBus
    {

        ObservableCollection<Product> productList;// = new ObservableCollection<Product>();
        ObservableCollection<Product> allProductList;// = new ObservableCollection<Product>();

        Product currentSelectedProduct = null;
        Guid selectedOptionGuid = new Guid();


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            EventBus.EventBus.Instance.Unregister(this);
        }

        public ClientOptionsProduct()
        {
            InitializeComponent();
        }

        public ClientOptionsProduct(Guid guid)
        {
            InitializeComponent();
            EventBus.EventBus.Instance.Register(this);
            gProductDetails.IsEnabled = (Global.ADUser.IsAdministrator == true || Global.ADUser.IsEmployee == true) ? true : false ; 

            selectedOptionGuid = guid;
            updateView(guid);
            loadAllProducts();
            if (productList.Count != 0)
            {
                updateInnerView(productList.ElementAt(0));
                lvOptionProducts.SelectedIndex = 0;
                currentSelectedProduct = productList.ElementAt(0);

            }

            btnMoveLeft.IsEnabled = false;
            btnMoveRight.IsEnabled = true;


        }

        public void DetachContent()
        {
            RemoveLogicalChild(Content);
        }

        private void lvOptionList_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                btnMoveLeft.IsEnabled = false;
                btnMoveRight.IsEnabled = true;
                try
                {
                    dynamic selectedClient = (ExpandoObject)item;
                    currentSelectedProduct = productList.First(product => product.GUID == selectedClient.Guid);
                    updateInnerView(currentSelectedProduct);
                }
                catch (InvalidOperationException exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, true, true);
                }
            }
        }

        private void lvAllProducts_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                btnMoveLeft.IsEnabled = true;
                btnMoveRight.IsEnabled = false;
                try
                {
                    dynamic selectedClient = (ExpandoObject)item;
                    currentSelectedProduct = allProductList.First(product => product.GUID == selectedClient.Guid);
                    updateInnerView(currentSelectedProduct);
                }
                catch (InvalidOperationException exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, true, true);
                }
            }
        }


        public void updateView(Guid guid)
        {
            try
            {
                ProductLazy productLazy = new ProductLazy(guid);
                productList = new ObservableCollection<Product>(productLazy.ProductList);
               

                List<ExpandoObject> productListView = new List<ExpandoObject>();
                foreach (Product product in productList)
                {
                    dynamic javascript = new ExpandoObject();
                    javascript.Name = product.Name;
                    javascript.InStock = product.InStock ? "Yes" : "No";
                    javascript.Guid = product.GUID;
                    productListView.Add(javascript);
                }

                lvOptionProducts.ItemsSource = productListView;

                updateOptionCost();
            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true, true);
            }
        }

        public void updateInnerView(Product product)
        {
            if (product != null)
            {
                txtProductName.Text = product.Name;
                txtDescription.Text = product.Description;
                txtProductPrice.Text = string.Format("R {0:0.00}", product.Price);
                txtWarrentyPeriod.Text = string.Format("{0} Months", product.WarrentyPeriod);
                txtInStock.Text = product.InStock ? "Yes" : "No";
                txtArrivalDate.Text = product.ArrivalDate.ToString("d MMMM, yyyy");
                txtProductCode.Text = product.ProductCode;
                txtType.Text = product.Type;
                txtManufacturer.Text = product.Manufacturer;
                txtModel.Text = product.Model;
                txtSerialNumber.Text = product.SerialNumber;
            }
        }

        public void loadAllProducts()
        {
            try
            {
                ProductLazy productLazy = new ProductLazy();
                List<Product> allOfDem = productLazy.loadAllProducts();
                allProductList = new ObservableCollection<Product>();
                foreach (Product prod in allOfDem)
                {
                    if (!productList.Contains(prod))
                    {
                        allProductList.Add(prod);
                    }
                }

                List<ExpandoObject> productListView = new List<ExpandoObject>();
                foreach (Product product in allProductList)
                {                    
                    dynamic javascript = new ExpandoObject();
                    javascript.Name = product.Name;
                    javascript.InStock = product.InStock ? "Yes" : "No";
                    javascript.Guid = product.GUID;
                    productListView.Add(javascript);                                      
                }

                lvAllProducts.ItemsSource = productListView;
            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true, true);
            }
        }

        private void btnMoveLeft_Click(object sender, RoutedEventArgs e)
        {
            if (currentSelectedProduct != null && !productList.Contains(currentSelectedProduct))
            {
                try
                {

                    OptionProduct optionToMove = new OptionProduct(selectedOptionGuid, currentSelectedProduct.GUID);
                    optionToMove.InsertProductToOption();
                    // Do SQL here

                    allProductList.Remove(currentSelectedProduct);
                    productList.Add(currentSelectedProduct);


                    lvAllProducts.ItemsSource = null;
                    List<ExpandoObject> productListViewAll = new List<ExpandoObject>();
                    foreach (Product product in allProductList)
                    {
                        dynamic javascript = new ExpandoObject();
                        javascript.Name = product.Name;
                        javascript.InStock = product.InStock ? "Yes" : "No";
                        javascript.Guid = product.GUID;
                        productListViewAll.Add(javascript);
                    }
                    lvAllProducts.ItemsSource = productListViewAll;
                    if (allProductList.Count != 0)
                    {
                        lvAllProducts.SelectedIndex = 0;
                        currentSelectedProduct = allProductList.ElementAt(0);
                        updateInnerView(allProductList.ElementAt(0));
                    }


                    lvOptionProducts.ItemsSource = null;
                    List<ExpandoObject> productListView = new List<ExpandoObject>();
                    foreach (Product product in productList)
                    {
                        dynamic javascript = new ExpandoObject();
                        javascript.Name = product.Name;
                        javascript.InStock = product.InStock ? "Yes" : "No";
                        javascript.Guid = product.GUID;
                        productListView.Add(javascript);
                    }

                    lvOptionProducts.ItemsSource = productListView;



                    updateOptionCost();
                    EventBus.EventBus.Instance.PostEvent(new CustomEvent("OptionInserted"));


                }
                catch (InvalidOperationException exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, false, true);
                }
            }
        }

        private void btnMoveRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentSelectedProduct != null && !allProductList.Contains(currentSelectedProduct))
            {
                try
                {

                    OptionProduct optionToMove = new OptionProduct(selectedOptionGuid, currentSelectedProduct.GUID);
                    optionToMove.removeProductFromOption();

                    productList.Remove(currentSelectedProduct);
                    allProductList.Add(currentSelectedProduct);


                    lvAllProducts.ItemsSource = null;
                    List<ExpandoObject> productListViewAll = new List<ExpandoObject>();
                    foreach (Product product in allProductList)
                    {
                        if (!productList.Contains(product))
                        {
                            dynamic javascript = new ExpandoObject();
                            javascript.Name = product.Name;
                            javascript.InStock = product.InStock ? "Yes" : "No";
                            javascript.Guid = product.GUID;
                            productListViewAll.Add(javascript);
                        }

                    }
                    lvAllProducts.ItemsSource = productListViewAll;



                    lvOptionProducts.ItemsSource = null;
                    List<ExpandoObject> productListView = new List<ExpandoObject>();
                    foreach (Product product in productList)
                    {
                        dynamic javascript = new ExpandoObject();
                        javascript.Name = product.Name;
                        javascript.InStock = product.InStock ? "Yes" : "No";
                        javascript.Guid = product.GUID;
                        productListView.Add(javascript);
                    }

                    lvOptionProducts.ItemsSource = productListView;
                    if (productList.Count != 0)
                    {
                        lvOptionProducts.SelectedIndex = 0;
                        currentSelectedProduct = productList.ElementAt(0);
                        updateInnerView(productList.ElementAt(0));
                    }

                    updateOptionCost();
                    EventBus.EventBus.Instance.PostEvent(new CustomEvent("OptionInserted"));

                    // do SQL query on specificProduct;
                }
                catch (InvalidOperationException exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, false, true);
                }
            }
        }

        public void updateOptionCost()
        {
            decimal cost = 0;
            foreach (Product product in productList)
            {
                cost += product.Price;
            }

            lblOptionCost.Content = string.Format("R {0:0.00}", cost);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            NavigationService.NavigateBack();
        }

        public void OnEvent(CustomEvent customEvent)
        {
            
        }
    }
}
