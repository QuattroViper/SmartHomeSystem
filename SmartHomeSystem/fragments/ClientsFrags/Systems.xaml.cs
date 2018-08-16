using ClassLibrary.classes;
using ClassLibrary.classes.Combined;
using ClassLibrary.classes.lazyLoad.client;
using ErrorHandler;
using EventBus;
using System;
using System.Collections.Generic;
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

namespace SmartHomeSystem.fragments.ClientsFrags
{
    /// <summary>
    /// Interaction logic for Systems.xaml
    /// </summary>
    public partial class Systems : Window, IDetachContent, INotifyClientChange
    {

        List<ClassLibrary.classes.System> systems = new List<ClassLibrary.classes.System>();
        ClassLibrary.classes.System currentSystem = null;
        OptionLazy currentOption = null;
        OptionLazy currentSelectedOption = null;
        List<OptionLazy> optionList = new List<OptionLazy>();
        public BorderViewModel ViewModel { get; set; }

        public Systems()
        {
            InitializeComponent();

        }

        public Systems(Guid guid)
        {
            InitializeComponent();

            updateView(guid);

            btnAddOption.IsEnabled = ( Global.ADUser.IsAdministrator == true || Global.ADUser.IsEmployee == true ) ? true : false;

            ViewModel = new BorderViewModel();

            addOptionGrid.DataContext = ViewModel;
        }
        

        private void lvClientSystems_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                
                try
                {
                    dynamic selectedClient = (ExpandoObject)item;
                    currentSystem = systems.First(system => system.Contract.GUID == selectedClient.Guid);
                    updateInnerView(currentSystem);
                }
                catch (InvalidOperationException exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, true, true);
                }
            }
        }

        private void listView_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {

                try
                {
                    dynamic selectedClient = (ExpandoObject)item;
                    currentSelectedOption = new OptionLazy();
                    currentSelectedOption.GUID = selectedClient.Guid;
                    // Needs to be moved to Products
                    //ClientOptions CO = new ClientOptions(selectedClient.Guid);
                    //CO.Show();

                    btnRemoveOption.IsEnabled = (Global.ADUser.IsAdministrator == true || Global.ADUser.IsEmployee == true) ? true : false;
                }
                catch (Exception exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, true, true);
                }

                
            }
        }

        public void updateInnerView() {
            txtContractIdentifier.Text = "No System";
            txtIsActive.Text = "No System";
            txtServiceLevel.Text = "No System";
            txtStartDate.Text = "No System";
            txtEndDate.Text = "No System";
            txtUpgradeOptions.Text = "No System";
            txtCritical.Text = "No System";
            txtWarning.Text = "No System";
            if (lvClientOptions.Items.Count != 0)
            {
                lvClientOptions.ItemsSource = null;
            }
            if (lvClientSystems.Items.Count != 0)
            {
                lvClientSystems.ItemsSource = null;
            }
            
        }

        public void updateInnerView(ClassLibrary.classes.System singleSystem)
        {
            if (singleSystem != null)
            {
                txtContractIdentifier.Text = singleSystem.Contract.ContractIdentifier;
                txtIsActive.Text = singleSystem.Contract.IsActive ? "Yes" : "No";
                txtServiceLevel.Text = singleSystem.Contract.ServiceLevel;
                txtStartDate.Text = singleSystem.Contract.StartDate.ToString("d MMMM, yyyy");
                txtEndDate.Text = singleSystem.Contract.EndDate.ToString("d MMMM, yyyy");
                txtUpgradeOptions.Text = singleSystem.Contract.UpgradeOptions;
                txtCritical.Text = singleSystem.Status.Citical == 1 ? singleSystem.Status.Messages : "None";
                txtWarning.Text = singleSystem.Status.Warning == 1 ? singleSystem.Status.Messages : "None";

                OptionLazy optionLoad = new OptionLazy(singleSystem.GUID);
                singleSystem.Options = optionLoad.OptionList;

                List<ExpandoObject> optionListView = new List<ExpandoObject>();
                foreach (OptionLazy option in singleSystem.Options)
                {
                    dynamic javascript = new ExpandoObject();
                    javascript.Name = option.Name;
                    javascript.Price = string.Format("R {0:0.00}", (option.CombinedPrice + option.Price)).EndsWith("00") ? string.Format("R {0}", Convert.ToInt32((option.CombinedPrice + option.Price))) : string.Format("R {0:0.00}", (option.CombinedPrice + option.Price));
                    javascript.Guid = option.GUID;
                    optionListView.Add(javascript);
                }

                lvClientOptions.ItemsSource = optionListView;
                if (optionListView.Count != 0)
                {
                    lvClientOptions.SelectedIndex = 0;
                }

                // string.Format("R {0:0.00}", accountLazy.CostPerMonth).EndsWith("00") ? string.Format("R {0}", Convert.ToInt32(accountLazy.CostPerMonth)) : string.Format("R {0:0.00}", accountLazy.CostPerMonth);
                //.ToString("d MMMM, yyyy");
            }
            else
            {
                // Error message
            }
        }

        public void DetachContent()
        {
            RemoveLogicalChild(Content);
        }

        public void updateView(Guid guid)
        {
            SystemLazy systemLazy = new SystemLazy(guid);
            systems = new List<ClassLibrary.classes.System>();
            systems = systemLazy.Systems;

            List<ExpandoObject> systemListView = new List<ExpandoObject>();

            int count = 0;

            foreach (ClassLibrary.classes.System sys in systems)
            {
                count++;
                dynamic javascript = new ExpandoObject();
                javascript.Name = string.Format("System {0}",count);
                javascript.Active = sys.Contract.IsActive ? "Yes" : "No" ;
                javascript.Guid = sys.Contract.GUID;
                systemListView.Add(javascript);
            }
            lvClientSystems.ItemsSource = systemListView;
            if (systems.Count != 0)
            {
                currentSystem = systems.First(system => system.GUID == systems.ElementAt(0).GUID);
                updateInnerView(currentSystem);
                lvClientSystems.SelectedIndex = 0;
            }
            else
            {
                updateInnerView();
            }
           

        }

        private void btnAddOption_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddOptionEnabled = true;
            updateView();
        }

        private void btnAddOptionfromList_Click(object sender, RoutedEventArgs e)
        {
            if (currentOption == null)
            {
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Please select an option first", CustomEvent.EventType.warning));
            }
                else
            {
                // SQL to add option to client
                SystemOption optionProduct = new SystemOption(currentSystem.GUID,currentOption.GUID);
                optionProduct.insertIntoSystemOption();

                updateOptions();

                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option successfully added", CustomEvent.EventType.accept));
                ViewModel.AddOptionEnabled = false;
            }
            
        }

        private void btnCancelOptionfromList_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddOptionEnabled = false;

        }

        public void updateOptions()
        {
            OptionLazy optionLoad = new OptionLazy(currentSystem.GUID);
            currentSystem.Options = optionLoad.OptionList;

            List<ExpandoObject> optionListView = new List<ExpandoObject>();
            foreach (OptionLazy option in currentSystem.Options)
            {
                dynamic javascript = new ExpandoObject();
                javascript.Name = option.Name;
                javascript.Price = string.Format("R {0:0.00}", (option.CombinedPrice + option.Price)).EndsWith("00") ? string.Format("R {0}", Convert.ToInt32((option.CombinedPrice + option.Price))) : string.Format("R {0:0.00}", (option.CombinedPrice + option.Price));
                javascript.Guid = option.GUID;
                optionListView.Add(javascript);
            }

            lvClientOptions.ItemsSource = optionListView;
            if (optionListView.Count != 0)
            {
                lvClientOptions.SelectedIndex = 0;
            }
        }

        public void updateView()
        {
            lvOptionsToChooseFrom.ItemsSource = null;
            OptionLazy optionLazy = new OptionLazy();

            optionList = optionLazy.LoadAllOptions();

            lvOptionsToChooseFrom.ItemsSource = optionList;

        }

        private void lvOptionsToChooseFrom_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {

                try
                {
                    //dynamic selectedClient = (ExpandoObject)item;

                    currentOption = (OptionLazy)item;


                }
                catch (Exception exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, true);
                }


            }
        }

        private void btnRemoveOption_Click(object sender, RoutedEventArgs e)
        {
            if (currentSelectedOption == null)
            {
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Please select an option first", CustomEvent.EventType.warning));
            }
                else
            {
                SystemOption systemOption = new SystemOption(currentSystem.GUID, currentSelectedOption.GUID);
                systemOption.removeFromClientOptions();

                updateOptions();
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Option successfully removed", CustomEvent.EventType.accept));
            }
            
        }
    }

    public class BorderViewModel : INotifyPropertyChanged
    {
        private bool addOptionEnabled = false;

        public bool AddOptionEnabled
        {
            get
            {
                return addOptionEnabled;
            }

            set
            {
                addOptionEnabled = value;
                NotifyPropertyChanged("AddOptionEnabled");
            }
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
