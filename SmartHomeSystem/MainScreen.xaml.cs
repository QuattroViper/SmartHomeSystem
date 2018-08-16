using ClassLibrary.classes;
using EventBus;
using SmartHomeSystem.calllcenter;
using SmartHomeSystem.client;
using SmartHomeSystem.fragments;
using SmartHomeSystem.fragments.productFrags;
using SmartHomeSystem.Schedules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SmartHomeSystem
{

    public class BorderViewModel : INotifyPropertyChanged
    {
        private bool notificationEnabled = false;

        public bool NotificationEnabled
        {
            get
            {
                return notificationEnabled;
            }

            set
            {
                notificationEnabled = value;
                NotifyPropertyChanged("NotificationEnabled");
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

    

    /// <summary>
    /// Interaction logic for MainScreen.xaml
    /// </summary>
    public partial class MainScreen : Window, IEventBus
    {

        public BorderViewModel ViewModel { get; set; }

        


        SolidColorBrush selectedBrush = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(156), Convert.ToByte(208), Convert.ToByte(49)));
        SolidColorBrush backgroundBrush = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(230), Convert.ToByte(231), Convert.ToByte(237)));

        Clients clientsWindow = null;
        ProductWindow productWindow = null;
        SchedulingWindow schedulingWindow = null;
        CallCenterWindow callcenterWindow = null;

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            EventBus.EventBus.Instance.Unregister(this);
        }

        public MainScreen()
        {
            InitializeComponent();
            loadEvents();
            backgroundBrush.Opacity = 0;

            EventBus.EventBus.Instance.Register(this);

            ViewModel = new BorderViewModel();

            notificationGrid.DataContext = ViewModel;

            // Color Codes
            // Blue ish = #272b40
            // Green = #9cd031
            // Selected #ebeef9
            // Equal / Unequal = #f4f6fa
            // Text Color = #FF595959
            // background = #e6e7ed



            if (Global.ADUser.IsAdministrator == true) this.Title = string.Format("Main Screen          {0} - Administrator",Global.ADUser.Name);
            if (Global.ADUser.IsEmployee == true) this.Title = string.Format("Main Screen          {0} - Employee", Global.ADUser.Name);
            if (Global.ADUser.IsTechnician == true) this.Title = string.Format("Main Screen          {0} - Technician", Global.ADUser.Name);



            Clients clientsWindow = new Clients();
            clientsWindow.DetachContent();
            fragmentWindow.Children.Add(clientsWindow.ClientMain);


        }

        void checkForAppointmentsAndErrors ()
        {
            Task.Run( () => { }).ConfigureAwait(false);
        }

        void checkForAppointmentsOverDate()
        {
            Task.Run(() => { }).ConfigureAwait(false);
        }

        void loadEvents()
        {
            spClients.MouseUp += SpClients_MouseUp;
            spProducts.MouseUp += SpProducts_MouseUp;
            spScheduler.MouseUp += SpScheduler_MouseUp;
            spCallCentre.MouseUp += SpCallCentre_MouseUp;
            spLogout.MouseUp += SpLogout_MouseUp;
        }



        void clearAllItems()
        {
            if (fragmentWindow.Children.Count != 0)
            {
                fragmentWindow.Children.RemoveAt(0);
            }        
            spClients.Background = backgroundBrush;
            spProducts.Background = backgroundBrush;
            spScheduler.Background = backgroundBrush;
            spCallCentre.Background = backgroundBrush;
        }

        private void SpLogout_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Global.ADUser = null;

            NavigationService.NavigateBack();
        }

        private void SpCallCentre_MouseUp(object sender, MouseButtonEventArgs e)
        {
            clearAllItems();
            spCallCentre.Background = selectedBrush;
            if (callcenterWindow == null)
            {
                callcenterWindow = new CallCenterWindow();
            }
            else
            {
                callcenterWindow.updateView();
            }
            callcenterWindow.DetachContent();
            fragmentWindow.Children.Add(callcenterWindow.CallcenterGrid);
        }

        private void SpScheduler_MouseUp(object sender, MouseButtonEventArgs e)
        {
            clearAllItems();
            spScheduler.Background = selectedBrush;
            if (schedulingWindow == null)
            {
                schedulingWindow = new SchedulingWindow();
            }
            else
            {
                schedulingWindow.updateView();
            }
            schedulingWindow.DetachContent();
            fragmentWindow.Children.Add(schedulingWindow.SchedulerMain);
            
        }

        private void SpProducts_MouseUp(object sender, MouseButtonEventArgs e)
        {
            clearAllItems();
            spProducts.Background = selectedBrush;
            if (productWindow == null)
            {
                productWindow = new ProductWindow();
            }
            productWindow.DetachContent();
            fragmentWindow.Children.Add(productWindow.OptionMain);
        }

        private void SpClients_MouseUp(object sender, MouseButtonEventArgs e)
        {
            clearAllItems();
            spClients.Background = selectedBrush;
            if (clientsWindow == null)
            {
                clientsWindow = new Clients();
            }            
            clientsWindow.DetachContent();
            fragmentWindow.Children.Add(clientsWindow.ClientMain);
        }

        public void OnEvent(CustomEvent customEvent)
        {
            if (customEvent.EventString.Equals("Notify"))
            {
                doWork(customEvent.EventMessage, customEvent.Type);
            }
        }

        private Task<bool> doNotificationAsync(string message, CustomEvent.EventType type)
        {
            SolidColorBrush critical = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(220), Convert.ToByte(53), Convert.ToByte(69)));
            SolidColorBrush warning = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(255), Convert.ToByte(193), Convert.ToByte(7)));
            SolidColorBrush accept = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(40), Convert.ToByte(167), Convert.ToByte(69)));
            //SolidColorBrush backgroundBrush = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(230), Convert.ToByte(231), Convert.ToByte(237)));

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            object locker = new object();
            object locker2 = new object();

            try
            {
                Task.Run(() => {

                    lock (locker)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate {

                            lock (locker2)
                            {
                                System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                                dispatcherTimer.Interval = new TimeSpan(0, 0, 3);

                                dispatcherTimer.Tick += (sernder, e) => { ViewModel.NotificationEnabled = false; };

                                dispatcherTimer.Start();

                                switch (type)
                                {
                                    case CustomEvent.EventType.warning:
                                    {
                                        notificationGrid.Background = warning;
                                        tbNotifyMessage.Text = message;
                                        ViewModel.NotificationEnabled = true;
                                    }
                                    break;
                                    case CustomEvent.EventType.critical:
                                    {
                                        notificationGrid.Background = critical;
                                        tbNotifyMessage.Text = message;
                                        ViewModel.NotificationEnabled = true;
                                    }
                                    break;
                                    case CustomEvent.EventType.accept:
                                    {
                                        notificationGrid.Background = accept;
                                        tbNotifyMessage.Text = message;
                                        ViewModel.NotificationEnabled = true;
                                    }
                                    break;
                                    default:
                                    break;
                                }
                            }

                        });
                    }

                    tcs.SetResult(true);

                });

                // set a timmer for how long the notification should be visible
                // return true when timer is done.

                return tcs.Task;
            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
            }

            return tcs.Task;

        }

        private async void doWork(string message, CustomEvent.EventType type)
        {
            bool result = await doNotificationAsync(message, type);
        }

        private void mainScreen_Closing(object sender, CancelEventArgs e)
        {
            NavigationService.NavigateBack();
        }

        private void image1_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Global.ADUser.IsAdministrator == true)
            {
                NavigationService.NavigateToWithoutHide(new SystemLoginsWindow());
            }
        }
    }
}
