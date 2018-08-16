using ClassLibrary.classes;
using ClassLibrary.classes.Combined;
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
using EventBus;

namespace SmartHomeSystem.Schedules
{
    /// <summary>
    /// Interaction logic for SchedulingWindow.xaml
    /// </summary>
    public partial class SchedulingWindow : Window, IDetachContent, IEventBus
    {

        Dictionary<ActiveDirectoryEmployee, AppointmentPriority> ActiveTechnicions = new Dictionary<ActiveDirectoryEmployee, AppointmentPriority>();
        ServiceDelivery serviceDelivery;

        public SchedulingWindow()
        {
            InitializeComponent();

            EventBus.EventBus.Instance.Register(this);

            updateView();

        }

        public void updateView()
        {
            serviceDelivery = new ServiceDelivery();

            ActiveTechnicions.Clear();
            lvActiveTechnicions.ItemsSource = null;
            lvUnassignedAppointments.ItemsSource = null;

            ActiveTechnicions = serviceDelivery.ActiveTechniciansList;
            lvActiveTechnicions.ItemsSource = ActiveTechnicions;
            lvUnassignedAppointments.ItemsSource = serviceDelivery.UnassignedAppointments;
        }

        public void DetachContent()
        {
            RemoveLogicalChild(Content);
        }

        private void lvUnassignedAppointments_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {

                try
                {
                    //dynamic selectedClient = (ExpandoObject)item;

                    AppointmentPriority selectedDictionaryItem = (AppointmentPriority)item;

                    NavigationService.NavigateToWithoutHide(new UnassignedWindow(selectedDictionaryItem, serviceDelivery.TechniciansList));

                }
                catch (Exception exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, true);
                }


            }
        }

        private void lvActiveTechnicions_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {

                try
                {
                    //dynamic selectedClient = (ExpandoObject)item;

                    KeyValuePair<ActiveDirectoryEmployee, AppointmentPriority> selectedDictionaryItem = (KeyValuePair<ActiveDirectoryEmployee, AppointmentPriority>)item;

                    Client client = new Client();
                    client = client.getClientOnAppointmentGuid(selectedDictionaryItem.Value.GUID);
                    

                    txtDate.Text = selectedDictionaryItem.Value.Time.ToString("d MMMM, yyyy hh:mm tt");
                    txtCost.Text = string.Format("R {0}", selectedDictionaryItem.Value.Cost);
                    txtOperation.Text = selectedDictionaryItem.Value.Operation;
                    switch(selectedDictionaryItem.Value.Priority)
                    {
                        case 0:
                        txtPriority.Text = string.Format("{0}  - Very Urgent", selectedDictionaryItem.Value.Priority);
                        break;
                        case 1:
                        txtPriority.Text = string.Format("{0}  - Very Urgent", selectedDictionaryItem.Value.Priority);
                        break;
                        case 2:
                        txtPriority.Text = string.Format("{0}  - Moderate Urgent", selectedDictionaryItem.Value.Priority);
                        break;
                        case 3:
                        txtPriority.Text = string.Format("{0}  - Medium Urgent", selectedDictionaryItem.Value.Priority);
                        break;
                        case 4:
                        txtPriority.Text = string.Format("{0}  - Least Urgent", selectedDictionaryItem.Value.Priority);
                        break;
                        case 5:
                        txtPriority.Text = string.Format("{0}  - Least Urgent", selectedDictionaryItem.Value.Priority);
                        break;
                        default:
                        break;
                    }
                    txtNameAndSurname.Text = client.Name + " " + client.Surname;
                    txtClientID.Text = client.ID;
                    txtClientIdenficator.Text = client.ClientIdetifier;
                    tbExtraDtails.Text = selectedDictionaryItem.Value.ExtraDetails;

                    chkConfirmed.IsChecked = true;
                    chkCompleted.IsChecked = false;


                }
                catch (Exception exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, true);
                }


            }
        }

        public void OnEvent(CustomEvent customEvent)
        {
            if (customEvent.EventString.Equals("AppointAddedToSchedule"))
            {
                updateView();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EventBus.EventBus.Instance.Unregister(this);
        }
    }
}
