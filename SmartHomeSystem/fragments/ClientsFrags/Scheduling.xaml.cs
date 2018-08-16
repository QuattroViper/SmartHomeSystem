using ClassLibrary.classes;
using ClassLibrary.classes.lazyLoad.client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SmartHomeSystem.fragments.ClientsFrags
{
    /// <summary>
    /// Interaction logic for Scheduling.xaml
    /// </summary>
    public partial class Scheduling : Window, IDetachContent, IEventBus
    {

        ObservableCollection<Appointment> appointments;
        Appointment currentAppointment = new Appointment();

        Guid selectedClientGuid = new Guid();

        public Scheduling()
        {
            InitializeComponent();

        }

        public Scheduling(Guid guid)
        {
            InitializeComponent();

            EventBus.EventBus.Instance.Register(this);

            selectedClientGuid = guid;

            btnDelete.IsEnabled = false;

            updateView(guid);
        }

        public void updateView(Guid guid)
        {
            AppointmentLazy appointmentLazy = new AppointmentLazy(guid);

            selectedClientGuid = guid;

            appointments = new ObservableCollection<Appointment>(appointmentLazy.AppointmentLazyList);
            if (appointments.Count != 0)
            {
                AppointmentGrid.IsEnabled = true;
                currentAppointment = appointments.ElementAt(0);
                lvAppointments.SelectedIndex = 0;

                lvAppointments.SelectedItem = currentAppointment;

                if (currentAppointment.Completed)
                {
                    chkCompleted.IsEnabled = false;
                }
                else
                {
                    chkCompleted.IsEnabled = true;
                }

                if (currentAppointment.Confirmed)
                {
                    chkConfirmed.IsEnabled = false;
                    if (currentAppointment.Completed)
                    {
                        chkCompleted.IsEnabled = false;
                    }
                    else
                    {
                        chkCompleted.IsEnabled = true;
                    }
                }
                else
                {

                    chkConfirmed.IsEnabled = true;
                    chkCompleted.IsEnabled = false;
                }




            } else
            {
                AppointmentGrid.IsEnabled = false;
            }
            lvAppointments.ItemsSource = appointments;
        }

        public void DetachContent()
        {
            RemoveLogicalChild(Content);
        }

        private void lblAddNewClient_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NavigationService.NavigateToWithoutHide(new NewAppointment(selectedClientGuid));
        }

        private void lvAppointments_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                AppointmentGrid.IsEnabled = true;
                btnDelete.IsEnabled = true;
                try
                {
                    //dynamic selectedClient = (ExpandoObject)item;

                    currentAppointment = (Appointment)item;
                    if (currentAppointment.Completed)
                    {
                        AppointmentGrid.IsEnabled = false;
                    }
                    else
                    {
                        AppointmentGrid.IsEnabled = true;
                    }

                    if (currentAppointment.Completed)
                    {
                        chkCompleted.IsEnabled = false;
                    }
                    else
                    {
                        chkCompleted.IsEnabled = true;
                    }

                    if (currentAppointment.Confirmed)
                    {
                        chkConfirmed.IsEnabled = false;
                        chkCompleted.IsEnabled = true;
                    }
                    else
                    {
                        chkConfirmed.IsEnabled = true;
                        chkCompleted.IsEnabled = false;
                    }


                }
                catch (Exception exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, true, true);
                }
            }
        }

        private void chkConfirmed_Checked(object sender, RoutedEventArgs e)
        {

            if (chkConfirmed.IsFocused)
            {
                currentAppointment.updateConfirmation();
                updateView(selectedClientGuid);
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Appointment confirmed", CustomEvent.EventType.accept));
            }

            if (currentAppointment.Confirmed)
            {
                chkConfirmed.IsEnabled = false;
            } 

                
            
            
        }

        private void chkCompleted_Checked(object sender, RoutedEventArgs e)
        {

            if (chkCompleted.IsFocused)
            {
                currentAppointment.updateCompleted();
                updateView(selectedClientGuid);
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Appointment completed", CustomEvent.EventType.accept));
            }

            if (currentAppointment.Completed)
            {
                chkCompleted.IsEnabled = false;
            } 
            
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            currentAppointment.deleteAppointment();
        }

        public void OnEvent(CustomEvent customEvent)
        {
            if (customEvent.EventString.Equals("NewAppointment"))
            {
                updateView(selectedClientGuid);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EventBus.EventBus.Instance.Unregister(this);
        }
    }
}
