using ClassLibrary.classes;
using ClassLibrary.classes.lazyLoad.client;
using ErrorHandle;
using ErrorHandler;
using EventBus;
using SmartHomeSystem.fragments.OptionFrags;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace SmartHomeSystem.fragments.ClientsFrags.ClientOptionsFrags
{
    /// <summary>
    /// Interaction logic for ClientOptionsOptionxaml.xaml
    /// </summary>
    public partial class ClientOptionsOptionxaml : Window, IDetachContent, IEventBus
    {

        SolidColorBrush correct = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(40), Convert.ToByte(167), Convert.ToByte(69)));
        SolidColorBrush inCorrect = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(220), Convert.ToByte(53), Convert.ToByte(69)));

        ObservableCollection<string> functionList = new ObservableCollection<string>();
        Option option = new Option();

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;


        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            EventBus.EventBus.Instance.Unregister(this);
        }

        public ClientOptionsOptionxaml()
        {
            InitializeComponent();

            EventBus.EventBus.Instance.Register(this);

            Loaded += ToolWindow_Loaded;

            txtDescription.IsReadOnly = false;
            txtInstallationTime.IsReadOnly = false;
            txtInstallCostByCompany.IsReadOnly = false;
            txtOptionName.IsReadOnly = false;
            txtOptionPrice.IsReadOnly = false;
            txtOptionGuid.IsReadOnly = false;

            txtOptionGuid.Text = Guid.NewGuid().ToString();
            txtOptionGuid.IsReadOnly = true;

            lvFunctions.ItemsSource = functionList;

            txtOptionName.DataContext = option.Name;
            txtDescription.DataContext = option.Description;
            txtInstallationTime.DataContext = option.InstallationTime;
            txtInstallCostByCompany.DataContext = option.InstallByCompanyCost;
            txtOptionPrice.DataContext = option.Price;

            this.DataContext = option;


            txtOptionName.BorderBrush = inCorrect;
            txtDescription.BorderBrush = inCorrect;
            txtInstallationTime.BorderBrush = inCorrect;
            txtInstallCostByCompany.BorderBrush = inCorrect;
            txtOptionPrice.BorderBrush = inCorrect;

            txtDescription.TextChanged += TxtDescription_TextChanged;
            txtInstallationTime.TextChanged += TxtInstallationTime_TextChanged;
            txtInstallCostByCompany.TextChanged += TxtInstallCostByCompany_TextChanged;
            txtOptionPrice.TextChanged += TxtOptionPrice_TextChanged;

            btnSave.Visibility = Visibility.Visible;
            btnUpdate.Visibility = Visibility.Hidden;

        }

        public ClientOptionsOptionxaml(Option optionToUpdate)
        {
            InitializeComponent();

            EventBus.EventBus.Instance.Register(this);

            Loaded += ToolWindow_Loaded;

            txtDescription.IsReadOnly = false;
            txtInstallationTime.IsReadOnly = false;
            txtInstallCostByCompany.IsReadOnly = false;
            txtOptionName.IsReadOnly = false;
            txtOptionPrice.IsReadOnly = false;
            txtOptionGuid.IsReadOnly = false;

            txtOptionGuid.Text = optionToUpdate.GUID.ToString();
            txtOptionGuid.IsReadOnly = true;

            optionToUpdate.Functions.ForEach(functionList.Add);
            lvFunctions.ItemsSource = functionList;

            txtOptionName.DataContext = optionToUpdate.Name;
            txtDescription.DataContext = optionToUpdate.Description;
            txtInstallationTime.DataContext = optionToUpdate.InstallationTime;
            txtInstallCostByCompany.DataContext = optionToUpdate.InstallByCompanyCost;
            txtOptionPrice.DataContext = optionToUpdate.Price;

            txtOptionName.Text = optionToUpdate.Name;
            txtDescription.Text = optionToUpdate.Description;
            txtInstallationTime.Text = optionToUpdate.InstallationTime.ToString();
            txtInstallCostByCompany.Text = optionToUpdate.InstallByCompanyCost.ToString();
            txtOptionPrice.Text = optionToUpdate.Price.ToString();

            //this.DataContext = option;


            txtOptionName.BorderBrush = correct;
            txtDescription.BorderBrush = correct;
            txtInstallationTime.BorderBrush = correct;
            txtInstallCostByCompany.BorderBrush = correct;
            txtOptionPrice.BorderBrush = correct;

            txtDescription.TextChanged += TxtDescription_TextChanged;
            txtInstallationTime.TextChanged += TxtInstallationTime_TextChanged;
            txtInstallCostByCompany.TextChanged += TxtInstallCostByCompany_TextChanged;
            txtOptionPrice.TextChanged += TxtOptionPrice_TextChanged;

            btnSave.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Visible;

        }



        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        public void DetachContent()
        {
            RemoveLogicalChild(Content);
        }
        

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!option.validateInputs<string>(txtOptionName.Text).IsValid)
            {
                //ErrorHandler.ErrorHandle.getInstance().handle(new CustomExceptionForUI(""),false,true);
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option Name should be a string and should not be empty.",CustomEvent.EventType.warning));
                return;
            }

            if (!option.validateInputs<string>(txtDescription.Text).IsValid)
            {
                //ErrorHandler.ErrorHandle.getInstance().handle(new CustomExceptionForUI("Option name should be a string and should not be empty."), false, true);
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option Description should be a string and should not be empty.", CustomEvent.EventType.warning));
                return;
            }

            if (!option.validateInputs<decimal>(txtOptionPrice.Text).IsValid)
            {
                //ErrorHandler.ErrorHandle.getInstance().handle(new CustomExceptionForUI("Option name should be a string and should not be empty."), false, true);
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option Price should be a decimal value and should not be empty.", CustomEvent.EventType.warning));
                return;
            }

            if (!option.validateInputs<decimal>(txtInstallCostByCompany.Text).IsValid)
            {
                //ErrorHandler.ErrorHandle.getInstance().handle(new CustomExceptionForUI("Option name should be a string and should not be empty."), false, true);
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option Cost by Company should be a decimal value and should not be empty.", CustomEvent.EventType.warning));
                return;
            }

            if (!option.validateInputs<int>(txtInstallationTime.Text).IsValid)
            {
                //ErrorHandler.ErrorHandle.getInstance().handle(new CustomExceptionForUI("Option name should be a string and should not be empty."), false, true);
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option Install time should be a integer value and should not be empty.", CustomEvent.EventType.warning));
                return;
            }

            if (functionList.Count == 0)
            {
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "the Option should atleast have 1 function.", CustomEvent.EventType.warning));
                return;
            }

            option.Functions = functionList.ToList();
            option.Name = txtOptionName.Text;
            option.Description = txtDescription.Text;
            option.Price = Convert.ToDecimal(txtOptionPrice.Text);
            option.InstallByCompanyCost = Convert.ToDecimal(txtInstallCostByCompany.Text);
            option.InstallationTime = Convert.ToInt32(txtInstallationTime.Text);
            option.GUID = new Guid(txtOptionGuid.Text);
            option.saveOptionToDatabase();

            EventBus.EventBus.Instance.PostEvent(new CustomEvent("OptionInserted"));
            NavigationService.NavigateBack();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.NavigateBack();
        }

        private void lvFunctions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //functionList.Add("");

            NavigationService.NavigateToWithoutHide(new OptionFunctionAdd(functionList));
        }

        public void OnEvent(CustomEvent customEvent)
        {
            if (customEvent.EventString.Equals("FunctionAdded"))
            {
                lvFunctions.Items.Refresh();
            }
        }

        private void txtOptionName_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtOptionName.BorderBrush = option.validateInputs<string>(txtOptionName.Text).IsValid ? correct : inCorrect;
        }

        private void TxtOptionPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtOptionPrice.BorderBrush = option.validateInputs<decimal>(txtOptionPrice.Text).IsValid ? correct : inCorrect; 
        }

        private void TxtInstallCostByCompany_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtInstallCostByCompany.BorderBrush = option.validateInputs<decimal>(txtInstallCostByCompany.Text).IsValid ? correct : inCorrect;
        }

        private void TxtInstallationTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtInstallationTime.BorderBrush = option.validateInputs<int>(txtInstallationTime.Text).IsValid ? correct : inCorrect;
        }

        private void TxtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtDescription.BorderBrush = option.validateInputs<string>(txtDescription.Text).IsValid ? correct : inCorrect;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!option.validateInputs<string>(txtOptionName.Text).IsValid)
            {
                //ErrorHandler.ErrorHandle.getInstance().handle(new CustomExceptionForUI(""),false,true);
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option Name should be a string and should not be empty.", CustomEvent.EventType.warning));
                return;
            }

            if (!option.validateInputs<string>(txtDescription.Text).IsValid)
            {
                //ErrorHandler.ErrorHandle.getInstance().handle(new CustomExceptionForUI("Option name should be a string and should not be empty."), false, true);
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option Description should be a string and should not be empty.", CustomEvent.EventType.warning));
                return;
            }

            if (!option.validateInputs<decimal>(txtOptionPrice.Text).IsValid)
            {
                //ErrorHandler.ErrorHandle.getInstance().handle(new CustomExceptionForUI("Option name should be a string and should not be empty."), false, true);
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option Price should be a decimal value and should not be empty.", CustomEvent.EventType.warning));
                return;
            }

            if (!option.validateInputs<decimal>(txtInstallCostByCompany.Text).IsValid)
            {
                //ErrorHandler.ErrorHandle.getInstance().handle(new CustomExceptionForUI("Option name should be a string and should not be empty."), false, true);
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option Cost by Company should be a decimal value and should not be empty.", CustomEvent.EventType.warning));
                return;
            }

            if (!option.validateInputs<int>(txtInstallationTime.Text).IsValid)
            {
                //ErrorHandler.ErrorHandle.getInstance().handle(new CustomExceptionForUI("Option name should be a string and should not be empty."), false, true);
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option Install time should be a integer value and should not be empty.", CustomEvent.EventType.warning));
                return;
            }

            if (functionList.Count == 0)
            {
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "the Option should atleast have 1 function.", CustomEvent.EventType.warning));
                return;
            }

            option.Functions = functionList.ToList();
            option.Name = txtOptionName.Text;
            option.Description = txtDescription.Text;
            option.Price = Convert.ToDecimal(txtOptionPrice.Text);
            option.InstallByCompanyCost = Convert.ToDecimal(txtInstallCostByCompany.Text);
            option.InstallationTime = Convert.ToInt32(txtInstallationTime.Text);
            option.GUID = new Guid(txtOptionGuid.Text);
            option.UpdateOption();

            EventBus.EventBus.Instance.PostEvent(new CustomEvent("OptionInserted"));
            EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option successfully updated", CustomEvent.EventType.accept));
            NavigationService.NavigateBack();
        }
    }
}
