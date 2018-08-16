using ClassLibrary.classes;
using ClassLibrary.classes.lazyLoad.client;
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
using System.Windows.Shapes;

namespace SmartHomeSystem.fragments.ClientsFrags
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : Window, IDetachContent, INotifyClientChange
    {
        public Details()
        {
            InitializeComponent();

            btnEditDetails.IsEnabled = (Global.ADUser.IsAdministrator == true || Global.ADUser.IsEmployee == true) ? true : false;
        }

        public void DetachContent()
        {
            RemoveLogicalChild(Content);
        }

        public void updateView(Guid guid)
        {
            
        }

        public void updateView(DetailsLazy details)
        {
            txtID.Text = details.ID;
            txtNameSurname.Text = string.Format("{0} {1}",details.Name,details.Surname);
            txtDob.Text = details.DateOfBirth.ToString("d MMMM, yyyy");
            txtGender.Text = details.Gender;

            txtEmail.Text = details.ContactDetails.EmailAddress;
            txtPhone.Text = details.ContactDetails.ContactNumber;
            radEmail.IsChecked = (details.ContactDetails.ContactMethods[0] != 1) ? true : true ;
            radMobile.IsChecked = (details.ContactDetails.ContactMethods[1] == 1);
            radSMS.IsChecked = (details.ContactDetails.ContactMethods[1] == 1);
            txtAndroidCode.Text = details.ContactDetails.AndroidDeviceID;
            txtIosCode.Text = details.ContactDetails.AppleDeviceID;
            txtUsername.Text = details.Login.Username;

            List<ExpandoObject> listviewList = new List<ExpandoObject>();

            foreach (Address addre in details.Addresses)
            {
                dynamic javascript = new ExpandoObject();
                javascript.Address = addre.Address1;
                javascript.Suburb = addre.Suburb;
                javascript.City = addre.City;
                javascript.Guid = addre.GUID;

                listviewList.Add(javascript);
            }

            lvAddresses.ItemsSource = listviewList;

            if (listviewList.Count != 0)
            {
                lvAddresses.SelectedIndex = 0;
            }


        }

        private void btnEditDetails_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
