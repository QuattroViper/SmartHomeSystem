using ClassLibrary.classes;
using ClassLibrary.classes.lazyLoad.client;
using SmartHomeSystem.fragments.ClientsFrags;
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
using System.Windows.Navigation;

using System.Windows.Shapes;
using EventBus;
using SmartHomeSystem.notification;
using ErrorHandle;
using SmartHomeSystem.client;

namespace SmartHomeSystem.fragments
{
    /// <summary>
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class Clients : Window, IDetachContent, IEventBus
    {

        SolidColorBrush selectedBrush = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(156), Convert.ToByte(208), Convert.ToByte(49)));
        SolidColorBrush backgroundBrush = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(230), Convert.ToByte(231), Convert.ToByte(237)));
        List<Client> clientList = new List<Client>();
        DetailsLazy currentClientDetails = new DetailsLazy();

        Details detailsWindow = null;
        ClientsFrags.Account accountwindow = null;
        Systems systemsWindow = null;
        Scheduling appointmentWindow = null;

        Guid currentClientGuid = new Guid();
        Guid currentAccountGuid = new Guid();

        bool clientSelected = false;

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            EventBus.EventBus.Instance.Unregister(this);
        }

        public Clients()
        {
            InitializeComponent();

            loadFragments();

            EventBus.EventBus.Instance.Register(this);


            loadEverything();

        }

        void loadEverything()
        {
            ClientLazy clientLazy = new ClientLazy();
            clientList = clientLazy.ClientList;

            List<ExpandoObject> listviewList = new List<ExpandoObject>();

            foreach (ClientLazy client in clientList)
            {
                dynamic javascript = new ExpandoObject();
                javascript.Name = client.Name + " " + client.Surname;
                javascript.Identifier = client.ClientIdetifier;
                javascript.clientGuid = client.ClientGuid;
                javascript.AccountGuid = client.AccountGuid;
                listviewList.Add(javascript);
            }

            lvClients.ItemsSource = listviewList;

            if (listviewList.Count > 0)
            {
                lblClientCount.Content = listviewList.Count < 10 ? "0" + listviewList.Count.ToString() : listviewList.Count.ToString();
                currentClientDetails = currentClientDetails.lazyLoad(clientList.ElementAt(0).GUID);
                lblClientNameSurname.Content = currentClientDetails.Name + " " + currentClientDetails.Surname;
                lblClientEmail.Content = currentClientDetails.ContactDetails.EmailAddress;
                lblClientNumber.Content = currentClientDetails.ContactDetails.ContactNumber;
                lvClients.SelectedIndex = 0;

            }
            else
            {
                lblClientCount.Content = "00";
            }

            if (detailsWindow != null)
            {
                detailsWindow.updateView(currentClientDetails);
                Global.currentClientGuid = currentClientDetails.GUID;
                currentClientGuid = currentClientDetails.GUID;
            }
        }

        private void listView_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                dynamic selectedClient = (ExpandoObject)item;
                currentClientDetails = currentClientDetails.lazyLoad(selectedClient.clientGuid);
                //currentClientDetails = new DetailsLazy(clientList.ElementAt(0).GUID);

                lblClientNameSurname.Content = currentClientDetails.Name + " " + currentClientDetails.Surname;
                lblClientEmail.Content = currentClientDetails.ContactDetails.EmailAddress;
                lblClientNumber.Content = currentClientDetails.ContactDetails.ContactNumber;

                Global.currentClientGuid = selectedClient.clientGuid;
                currentClientGuid = selectedClient.clientGuid;
                currentAccountGuid = selectedClient.AccountGuid;

                if (detailsWindow != null)
                {
                    detailsWindow.updateView(currentClientDetails);
                }

                if (accountwindow != null)
                {
                    accountwindow.updateView(currentAccountGuid);
                }

                if (systemsWindow != null)
                {
                    systemsWindow.updateView(currentClientGuid);
                }

                if (appointmentWindow != null)
                {
                    appointmentWindow.updateView(currentClientGuid);
                }



                clientSelected = true;
            }
        }

        void loadFragments()
        {
            detailsWindow = new Details();
            detailsWindow.DetachContent();
            ClientFragmentHolder.Children.Add(detailsWindow.DetailFragment);
        }

        public void DetachContent()
        {
            RemoveLogicalChild(Content);
        }

        private void label8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (clientSelected)
            {
                setSelectedToNone();
                label8.Background = selectedBrush;
                ClientFragmentHolder.Children.RemoveAt(0);
                detailsWindow.DetachContent();
                ClientFragmentHolder.Children.Add(detailsWindow.DetailFragment);
            }
            else
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(new CustomExceptionForUI("Please select a Client first."),false,true);

                //EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Please select a client first", CustomEvent.EventType.accept));
                // Show error message. A android style popup

            }
            
            
        }

        private void lblAccount_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (clientSelected)
            {
                setSelectedToNone();
                lblAccount.Background = selectedBrush;
                ClientFragmentHolder.Children.RemoveAt(0);
                accountwindow = new ClientsFrags.Account(currentAccountGuid);
                accountwindow.DetachContent();
                ClientFragmentHolder.Children.Add(accountwindow.AccountFragment);
            }
            else
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(new CustomExceptionForUI("Please select a Client first."), false, true);
                //EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Please select a client first", CustomEvent.EventType.critical));
                // Show error message. A android style popup
            }
        }

        private void lblSystems_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (clientSelected)
            {
                setSelectedToNone();
                lblSystems.Background = selectedBrush;
                ClientFragmentHolder.Children.RemoveAt(0);
                systemsWindow = new Systems(currentClientGuid);
                systemsWindow.DetachContent();
                ClientFragmentHolder.Children.Add(systemsWindow.SystemsFragment);
            }
            else
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(new CustomExceptionForUI("Please select a Client first."), false, true);
                //EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify","Please select a client first",CustomEvent.EventType.warning));
                // postEvent here
                // Show error message. A android style popup
            }      
        }



        private void lblAppointments_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (clientSelected)
            {
                setSelectedToNone();
                lblAppointments.Background = selectedBrush;
                ClientFragmentHolder.Children.RemoveAt(0);
                appointmentWindow  = new Scheduling(currentClientGuid);
                appointmentWindow.DetachContent();
                ClientFragmentHolder.Children.Add(appointmentWindow.SchedulingFragment);
            }
            else
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(new CustomExceptionForUI("Please select a Client first."), false, true);
                //EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Please select a client first", CustomEvent.EventType.warning));
                // Show error message. A android style popup
            }
        }

        void setSelectedToNone()
        {
            label8.Background = backgroundBrush;
            lblAccount.Background = backgroundBrush;
            lblSystems.Background = backgroundBrush;
            lblAppointments.Background = backgroundBrush;
        }

        public void OnEvent(CustomEvent customEvent)
        {
            if (customEvent.EventString.Equals("NewClient"))
            {
                loadEverything();
            }
        }

        private void lblAddNewClient_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NavigationService.NavigateToWithoutHide(new ClientInsert());
        }
    }




}
