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
using EventBus;

namespace SmartHomeSystem.fragments.ClientsFrags
{
    /// <summary>
    /// Interaction logic for NewAppointment.xaml
    /// </summary>
    public partial class NewAppointment : Window, IEventBus
    {

        Appointment appointment = new Appointment();

        public NewAppointment()
        {
            InitializeComponent();
        }

        public NewAppointment(Guid guid)
        {
            InitializeComponent();

            EventBus.EventBus.Instance.Register(this);

            txtCost.Text = "R 80";
            appointment.Cost = 80m;
            appointment.ClientGUID = guid;
            appointment.GUID = Guid.NewGuid();
            appointment.Completed = false;
            appointment.Confirmed = false;
            appointment.Time = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 7,0,0);
            appointment.Operation = "Routine";
        }


        private void chbOperation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            appointment.Operation = ((e.AddedItems[0] as ComboBoxItem).Content as string);



            switch (appointment.Operation)
            {
                case "Routine":
                    txtCost.Text = "R 80";
                    appointment.Cost = 80m;
                    break;
                case "Inspection":
                    txtCost.Text = "R 120";
                    appointment.Cost = 120m;
                    break;
                case "Update":
                    txtCost.Text = "R 20";
                    appointment.Cost = 20m;
                    break;
                case "Upgrade":
                    txtCost.Text = "R 200";
                    appointment.Cost = 200m;
                    break;
                case "Installation":
                    txtCost.Text = "R 300 + Option Cost";
                    appointment.Cost = 300m;
                    break;
                case "Removal":
                    txtCost.Text = "R 10";
                    appointment.Cost = 10m;
                    break;
                default:
                    break;
            }
        }

        private void chbTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DateTime date = dpAppointmentDate.SelectedDate ?? DateTime.UtcNow;

            switch ((e.AddedItems[0] as ComboBoxItem).Content as string) {
                case "07:00":
                appointment.Time = new DateTime(date.Year, date.Month, date.Day, 7, 0, 0);
                break;
                case "08:00":
                appointment.Time = new DateTime(date.Year, date.Month, date.Day, 8, 0, 0);
                break;
                case "09:00":
                appointment.Time = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0);
                break;
                case "10:00":
                appointment.Time = new DateTime(date.Year, date.Month, date.Day, 10, 0, 0);
                break;
                case "11:00":
                appointment.Time = new DateTime(date.Year, date.Month, date.Day, 11, 0, 0);
                break;
                case "12:00":
                appointment.Time = new DateTime(date.Year, date.Month, date.Day, 12, 0, 0);
                break;
                case "13:00":
                appointment.Time = new DateTime(date.Year, date.Month, date.Day, 13, 0, 0);
                break;
                case "14:00":
                appointment.Time = new DateTime(date.Year, date.Month, date.Day, 14, 0, 0);
                break;
                case "15:00":
                appointment.Time = new DateTime(date.Year, date.Month, date.Day, 15, 0, 0);
                break;
                case "16:00":
                appointment.Time = new DateTime(date.Year, date.Month, date.Day, 16, 0, 0);
                break;
                case "17:00":
                appointment.Time = new DateTime(date.Year, date.Month, date.Day, 17, 0, 0);
                break;

                default:
                appointment.Time = new DateTime(date.Year, date.Month, date.Day, 7, 0, 0);
                break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.NavigateBack();
            EventBus.EventBus.Instance.Unregister(this);
        }

        private void btnInsertAppointment_Click(object sender, RoutedEventArgs e)
        {
            appointment.ExtraDetails = tbExtraDetails.Text;

            if (appointment.Date != null)
            {
                appointment.insertNewAppointment();
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Appointment created", CustomEvent.EventType.accept));
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("NewAppointment"));
                NavigationService.NavigateBack();
            } else
            {
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify","Please select a appointment date", CustomEvent.EventType.warning));
            }
        }

        private void chkConfirmed_Checked(object sender, RoutedEventArgs e)
        {
            appointment.Confirmed = true;
        }

        private void chkConfirmed_Unchecked(object sender, RoutedEventArgs e)
        {
            appointment.Confirmed = false;
        }

        private void dpAppointmentDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            appointment.Date = dpAppointmentDate.SelectedDate ?? DateTime.UtcNow;
        }

        public void OnEvent(CustomEvent customEvent)
        {
            
        }
    }
}
