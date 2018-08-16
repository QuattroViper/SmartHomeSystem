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
    /// Interaction logic for UnassignedWindow.xaml
    /// </summary>
    public partial class UnassignedWindow : Window, IEventBus
    {

        AppointmentPriority appointment = null;
        List<TechnicianDBInfo> techniciansList = null;
        TechnicianDBInfo currentSelectedTechnician = null;

        public UnassignedWindow()
        {
            InitializeComponent();
        }

        public UnassignedWindow(AppointmentPriority appointment, List<TechnicianDBInfo> techniciansList)
        {
            InitializeComponent();

            EventBus.EventBus.Instance.Register(this);

            this.appointment = appointment;
            this.techniciansList = techniciansList;

            txtCost.Text = string.Format("R {0}", appointment.Cost);
            txtOperation.Text = appointment.Operation;
            switch (appointment.Priority)
            {
                case 0:
                txtPriority.Text = string.Format("{0}  - Very Urgent", appointment.Priority);
                break;
                case 1:
                txtPriority.Text = string.Format("{0}  - Very Urgent", appointment.Priority);
                break;
                case 2:
                txtPriority.Text = string.Format("{0}  - Moderate Urgent", appointment.Priority);
                break;
                case 3:
                txtPriority.Text = string.Format("{0}  - Medium Urgent", appointment.Priority);
                break;
                case 4:
                txtPriority.Text = string.Format("{0}  - Least Urgent", appointment.Priority);
                break;
                case 5:
                txtPriority.Text = string.Format("{0}  - Least Urgent", appointment.Priority);
                break;
                default:
                break;
            }
            txtDate.Text = appointment.Time.ToString("d MMMM, yyyy hh:mm tt");
            tbExtraDtails.Text = appointment.ExtraDetails;

            lvAllTechnicians.ItemsSource = techniciansList;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EventBus.EventBus.Instance.Unregister(this);
            NavigationService.NavigateBack();
        }

        private void lvAllTechnicians_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {

                try
                {
                    //dynamic selectedClient = (ExpandoObject)item;

                    TechnicianDBInfo selectedDictionaryItem = (TechnicianDBInfo)item;

                    TechnicianSchedule technicianSchedule = new TechnicianSchedule();

                    currentSelectedTechnician = selectedDictionaryItem;

                    bool isAvailable = technicianSchedule.checkIfTechnicianIsAvailable(appointment.Time, selectedDictionaryItem.ADTechnician.GUID ?? Guid.NewGuid());

                    txtNameSurname.Text = selectedDictionaryItem.ADTechnician.Name;

                    

                    if (isAvailable)
                    {
                        txtAvailable.Text = "Yes";
                        btnAssignTechnician.IsEnabled = true;
                    } else
                    {
                        txtAvailable.Text = "No, Slot unavailable";
                        btnAssignTechnician.IsEnabled = false;
                    }
                    
                    txtAppointmentsDone.Text = string.Format("{0} done",selectedDictionaryItem.TotalAppointments);

                }
                catch (Exception exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, true);
                }


            }
        }

        private void btnAssignTechnician_Click(object sender, RoutedEventArgs e)
        {
            TechnicianSchedule technicianSchedule = new TechnicianSchedule(Guid.NewGuid(), currentSelectedTechnician.ADTechnician.GUID?? new Guid(), appointment.GUID);
            technicianSchedule.insertTechnicianAppointment();
            EventBus.EventBus.Instance.PostEvent(new CustomEvent("AppointAddedToSchedule"));
            EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify","Appointment assigned to Technician", CustomEvent.EventType.accept));
            NavigationService.NavigateBack();
        }

        public void OnEvent(CustomEvent customEvent)
        {
            
        }
    }
}
