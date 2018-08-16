using ClassLibrary.classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartHomeSystem.calllcenter
{
    /// <summary>
    /// Interaction logic for CallCenterWindow.xaml
    /// </summary>
    public partial class CallCenterWindow : Window, IDetachContent
    {

        Client clientSearcher = new Client();
        Client client = null;
        double duration = 0;
        System.Timers.Timer timer = new System.Timers.Timer();
        Stopwatch stopwatch = new Stopwatch();

        SolidColorBrush red = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(220), Convert.ToByte(53), Convert.ToByte(69)));
        SolidColorBrush green = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(40), Convert.ToByte(167), Convert.ToByte(69)));

        public CallCenterWindow()
        {
            InitializeComponent();
            updateView();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;

            btnAnswer.Background = green;
            btnEnd.Background = red;

        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {


                TimeSpan ts = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);

                txtCallTime.Text = string.Format("{0:0} Hours, {1:0} Minutes, {2:0} Seconds", ts.Hours, ts.Minutes ,ts.Seconds );
            });
        }

        public void DetachContent()
        {
            RemoveLogicalChild(Content);
        }

        public void updateView()
        {

        }

        private void txtClientIdentifier_TextChanged(object sender, TextChangedEventArgs e)
        {

            
        }

        private void cbClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAnswer_Click(object sender, RoutedEventArgs e)
        {
            btnAnswer.IsEnabled = false;
            //btnEnd.IsEnabled = true;
            btnSearch.IsEnabled = true;
            txtClientIdentifier.IsEnabled = true;
            timer.Start();
            stopwatch.Start();
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            stopwatch.Stop();
            duration = Math.Round(stopwatch.ElapsedMilliseconds / 1000.0);



            CallInformation callInformation = new CallInformation(Guid.NewGuid(), new TextRange(rtbMemo.Document.ContentStart, rtbMemo.Document.ContentEnd).Text, duration, RandomString(6), Global.ADUser.GUID ?? Guid.NewGuid(), ((Person)client).GUID);
            callInformation.insertCallToDatabase();

            btnAnswer.IsEnabled = true;
            btnEnd.IsEnabled = false;
            btnSearch.IsEnabled = false;
            rtbMemo.IsEnabled = false;
            txtClientIdentifier.Clear();
            txtClientSelected.Clear();
            rtbMemo.Document.Blocks.Clear();
            stopwatch.Reset();
            txtCallTime.Clear();
            client = null;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            client = clientSearcher.getClientOnIdentifierOrID(txtClientIdentifier.Text);
            if (client != null)
            {
                rtbMemo.IsEnabled = true;
                txtClientSelected.Text = client.Name + " " + client.Surname;
                txtClientIdentifier.IsEnabled = false;
                btnSearch.IsEnabled = false;
                btnEnd.IsEnabled = true;
            } else
            {
                txtClientSelected.Text = "Client not found";
            }
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
