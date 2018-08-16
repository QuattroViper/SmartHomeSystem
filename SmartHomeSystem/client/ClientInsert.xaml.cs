using ClassLibrary.classes;
using ClassLibrary.classes.validation;
using ClassLibrary.functions;
using SmartHomeSystem.validation;
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
using EventBus;

namespace SmartHomeSystem.client
{
    /// <summary>
    /// Interaction logic for ClientInsert.xaml
    /// </summary>
    public partial class ClientInsert : Window, IEventBus
    {
        public BorderViewModel ViewModel { get; set; }
        SolidColorBrush warning = new SolidColorBrush(Color.FromArgb(0xFF, Convert.ToByte(255), Convert.ToByte(193), Convert.ToByte(7)));
        int count = 0;

        Client client = new Client();
        Contact contact = new Contact();
        Address address = new Address();
        Card card = new Card();
        ClassLibrary.classes.System system = new ClassLibrary.classes.System();

        bool activeChecked = false;
        ContractIdentifier contractIdentifier = new ContractIdentifier("A", "A");

        public ClientInsert()
        {
           
            InitializeComponent();

            EventBus.EventBus.Instance.Register(this);

            ViewModel = new BorderViewModel();

            notifyGrid.DataContext = ViewModel;

            ClientMainInfoGrid.Visibility = Visibility.Visible;
            btnPrevious.IsEnabled = false;
            radEmail.IsChecked = true;

            txtClientIdentifier.Text = generateClientIdentifier();
            txtRegisteredOn.Text = DateTime.UtcNow.ToString("d MMMM, yyyy");
            txtContractIdentifier.Text = generateContractIdentifier();
        }

        string generateContractIdentifier()
        {
            return contractIdentifier.ToString();
        }

        string generateClientIdentifier()
        {
            string identifier = "";
            StringBuilder builder = new StringBuilder();

            Random rand = new Random();

            string first = "ABCDE";

            builder.Append(first[rand.Next(0,4)]);
            
            int number = rand.Next(0,999999);

            for (int i = 0; i < 8 - number.ToString().Length; i++)
            {
                builder.Append("0");
            }

            builder.Append(number.ToString());

            identifier = builder.ToString();

            return identifier;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EventBus.EventBus.Instance.Unregister(this);
            NavigationService.NavigateBack();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NotificationEnabled = false;

            if (count != 0)
            {
                if ((count == 0 && validateMainInfo()) || (count == 1 && validateAddressInfo()) || (count == 2 && validateAccountInfo()) || (count == 3 && validateSystemInfo()))
                {
                    count--;
                }

            } 
            
            if (count == 0)
            {
                ClientMainInfoGrid.Visibility = Visibility.Visible;

                ClientAddressInfoGrid.Visibility = Visibility.Hidden;
                ClientAccountInfoGrid.Visibility = Visibility.Hidden;
                ClientSystemInfoGrid.Visibility = Visibility.Hidden;
                btnPrevious.IsEnabled = false;
                btnNext.IsEnabled = true;
                btnNext.Visibility = Visibility.Visible;
                btnInsert.Visibility = Visibility.Hidden;
                //count--;
            }
                else if (count == 1)
            {
                ClientAddressInfoGrid.Visibility = Visibility.Visible;

                ClientMainInfoGrid.Visibility = Visibility.Hidden;
                ClientAccountInfoGrid.Visibility = Visibility.Hidden;
                ClientSystemInfoGrid.Visibility = Visibility.Hidden;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                btnNext.Visibility = Visibility.Visible;
                btnInsert.Visibility = Visibility.Hidden;
                //count--;
            }
                else if (count == 2)
            {
                ClientAccountInfoGrid.Visibility = Visibility.Visible;

                ClientMainInfoGrid.Visibility = Visibility.Hidden;
                ClientAddressInfoGrid.Visibility = Visibility.Hidden;
                ClientSystemInfoGrid.Visibility = Visibility.Hidden;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                btnNext.Visibility = Visibility.Visible;
                btnInsert.Visibility = Visibility.Hidden;
                //count--;
            }
                else if (count == 3)
            {
                ClientSystemInfoGrid.Visibility = Visibility.Visible;

                ClientMainInfoGrid.Visibility = Visibility.Hidden;
                ClientAddressInfoGrid.Visibility = Visibility.Hidden;
                ClientAccountInfoGrid.Visibility = Visibility.Hidden;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = false;
                btnNext.Visibility = Visibility.Visible;
                btnInsert.Visibility = Visibility.Hidden;
                //count--;
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NotificationEnabled = false;

            if (count != 3)
            {
                if ((count == 0 && validateMainInfo()) || (count == 1 && validateAddressInfo()) || (count == 2 && validateAccountInfo()) || (count == 3 && validateSystemInfo()))
                {
                    count++;
                }
                
               
            }

            if (count == 0)
            {
                
                ClientMainInfoGrid.Visibility = Visibility.Visible;

                ClientAddressInfoGrid.Visibility = Visibility.Hidden;
                ClientAccountInfoGrid.Visibility = Visibility.Hidden;
                ClientSystemInfoGrid.Visibility = Visibility.Hidden;
                btnNext.IsEnabled = true;
                btnPrevious.IsEnabled = false;
                btnNext.Visibility = Visibility.Visible;
                btnInsert.Visibility = Visibility.Hidden;
            }
            else if (count == 1)
            {

                ClientAddressInfoGrid.Visibility = Visibility.Visible;

                ClientMainInfoGrid.Visibility = Visibility.Hidden;
                ClientAccountInfoGrid.Visibility = Visibility.Hidden;
                ClientSystemInfoGrid.Visibility = Visibility.Hidden;
                btnNext.IsEnabled = true;
                btnPrevious.IsEnabled = true;
                btnNext.Visibility = Visibility.Visible;
                btnInsert.Visibility = Visibility.Hidden;
            }
            else if (count == 2)
            {
                ClientAccountInfoGrid.Visibility = Visibility.Visible;

                ClientMainInfoGrid.Visibility = Visibility.Hidden;
                ClientAddressInfoGrid.Visibility = Visibility.Hidden;
                ClientSystemInfoGrid.Visibility = Visibility.Hidden;
                btnNext.IsEnabled = true;
                btnPrevious.IsEnabled = true;
                btnNext.Visibility = Visibility.Visible;
                btnInsert.Visibility = Visibility.Hidden;

            }
            else if (count == 3)
            {
                ClientSystemInfoGrid.Visibility = Visibility.Visible;

                ClientMainInfoGrid.Visibility = Visibility.Hidden;
                ClientAddressInfoGrid.Visibility = Visibility.Hidden;
                ClientAccountInfoGrid.Visibility = Visibility.Hidden;
                btnNext.IsEnabled = false;
                btnPrevious.IsEnabled = true;
                btnNext.Visibility = Visibility.Hidden;
                btnInsert.Visibility = Visibility.Visible;

            }
        }

        bool validateMainInfo()
        {           

            if (txtClientSAID.Text.Length != 13)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Client ID is not a valid ID";
                return false;
            }

            if (!client.validateInputs<string>(txtClientName.Text).IsValid)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Client name is not a valid string";
                return false;
            }

            if (!client.validateInputs<string>(txtSurname.Text).IsValid)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Client surname is not a valid string";
                return false;
            }

            if (!contact.validateEmail(txtEmailAddress.Text))
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Client email is not a valid email address";
                return false;
            }

            if (!client.validateInputs<decimal>(txtPhoneNumber.Text).IsValid || txtPhoneNumber.Text.Length != 10)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Client phone number is not a valid and 10 numbers long";
                return false;
            }

            ViewModel.NotificationEnabled = false;

            client.GUID = Guid.NewGuid();
            client.Name = txtClientName.Text;
            client.Surname = txtSurname.Text;
            client.ID = txtClientSAID.Text;
            client.ClientIdetifier = txtClientIdentifier.Text;
            client.ContactDetails = new Contact(Guid.NewGuid(), txtPhoneNumber.Text, txtEmailAddress.Text, "0", "0", new int[] { radEmail.IsChecked == true ? 1 : 0, radMobile.IsChecked == true ? 1: 0, radSMS.IsChecked == true ? 1 : 0});
            client.Login = new ClassLibrary.classes.Login();

            return true;

        }

        bool validateAddressInfo()
        {

            if (!address.validateInputs<string>(txtStreetAddressOne.Text).IsValid)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Street one is not a valid street name";
                return false;
            }

            if (!address.validateInputs<string>(txtSubUrb.Text).IsValid)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Suburb is not a valid suburb";
                return false;
            }

            if (!address.validateInputs<decimal>(txtPostalCode.Text).IsValid)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Postal code is not a valid code";
                return false;
            }

            if (!address.validateInputs<string>(txtCity.Text).IsValid)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "City is not a valid city";
                return false;
            }

            if (!address.validateInputs<string>(txtCountry.Text).IsValid)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Country is not a valid country";
                return false;
            }

            client.Addresses = new List<Address> { new Address(Guid.NewGuid(), txtStreetAddressOne.Text, txtStreetAddressTwo.Text, txtSubUrb.Text, txtPostalCode.Text, txtCity.Text, txtCountry.Text) };


            return true;

        }

        bool validateSystemInfo()
        {
            if (dpBeginDate.SelectedDate == null)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Begin date must be after today's date";
                return false;
            }

            if (dpEndDate.SelectedDate == null)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "End date must be after begin date";
                return false;
            }

            client.Systems = new List<ClassLibrary.classes.System>() { new ClassLibrary.classes.System(Guid.NewGuid(), new List<Option>(), new Status(), new Contract(Guid.NewGuid(), activeChecked, txtContractIdentifier.Text, chbServiceLevel.Text.ToString(), dpBeginDate.SelectedDate ?? DateTime.UtcNow, dpEndDate.SelectedDate ?? DateTime.UtcNow, chbUpgradeOptions.Text.ToString())) };

            return true;
        }

        bool validateAccountInfo()
        {


            if (!card.validateInputs<string>(txtAccountType.Text).IsValid)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Account Type is not a valid type";
                return false;
            }

            if (!card.validateInputs<string>(txtCardType.Text).IsValid)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Card Payment Type is not a valid type";
                return false;
            }

            if (!card.validateInputs<string>(txtCardHolderName.Text).IsValid)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Account Holder is not valid";
                return false;
            }

            if (dpExpireDate.SelectedDate.Value.CompareTo(DateTime.UtcNow) == 0)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Expire date must be after today's date";
                return false;
            }

            if (!card.validateInputs<string>(txtBankName.Text).IsValid)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Bank name is not a valid";
                return false;
            }

            if (!card.validateInputs<decimal>(txtCardNumber.Text).IsValid || txtCardNumber.Text.Length != 16)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Card Number is not a valid number and 16 numbers long";
                return false;
            }

            if (!card.validateInputs<int>(txtCardCVV.Text).IsValid || txtCardCVV.Text.Length != 3)
            {
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Card CVV is not a valid number and 3 numbers long";
                return false;
            }

            client.Account = new Account(Guid.NewGuid(), txtAccountType.Text, 0m, new Dictionary<string, string>(), false, 100m, DateTime.UtcNow, new Card(Guid.NewGuid(), txtBankName.Text, txtCardType.Text, txtCardNumber.Text, dpExpireDate.SelectedDate ?? DateTime.UtcNow, txtCardHolderName.Text, txtCardCVV.Text ) );

            return true;
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            validateSystemInfo();
            if (client.insertClient())
            {
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Client successfully created", CustomEvent.EventType.accept));
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("NewClient"));
                
                NavigationService.NavigateBack();
            } else
            {
                EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Could not create client", CustomEvent.EventType.critical));
                ViewModel.NotificationEnabled = true;
                notifyGrid.Background = warning;
                tbError.Text = "Could not create client";
            }
            
        }

        private void txtClientSAID_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check if it's a valid ID first
            if (txtClientSAID.Text.Length == 13)
            {
                Dictionary<string, object> decodedID = functions.decodeID(txtClientSAID.Text);
                DateTime bornDate = new DateTime();
                bornDate = (DateTime)decodedID["date"];
                txtClientDob.Text = bornDate.ToString("d MMMM, yyyy");
            }
        }

        private void chkActiveDirectory_Checked(object sender, RoutedEventArgs e)
        {
            activeChecked = true;
        }

        private void chkActiveDirectory_Unloaded(object sender, RoutedEventArgs e)
        {
            activeChecked = false;
        }

        private void chbServiceLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            contractIdentifier.ServiceLevel = alphabet[chbServiceLevel.SelectedIndex].ToString();
            txtContractIdentifier.Text = contractIdentifier.ToString();
        }

        private void chbUpgradeOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            contractIdentifier.ContractType = alphabet[chbUpgradeOptions.SelectedIndex].ToString();
            txtContractIdentifier.Text = contractIdentifier.ToString();
        }

        public void OnEvent(CustomEvent customEvent)
        {
            
        }
    }




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
}
