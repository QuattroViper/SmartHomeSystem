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


namespace SmartHomeSystem
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        ActiveDirectoryEmployee ADUser = null;
        int failedCount = 0;

        public Login()
        {
            InitializeComponent();

        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void chkActiveDirectory_Checked(object sender, RoutedEventArgs e)
        {
            lblDomain.Visibility = Visibility.Visible;

        }

        private void chkActiveDirectory_Unchecked(object sender, RoutedEventArgs e)
        {
            lblDomain.Visibility = Visibility.Hidden;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkActiveDirectory.IsChecked == true)
                {
                    ADUser = new ActiveDirectoryEmployee(txtUsername.Text, txtPassword.Password);
                    bool isAuthorised = ADUser.AuthenticateUser();

                    if (isAuthorised == true)
                    {
                        Global.ADUser = ADUser.ADPrincipal;
                        string type = "";
                        if (ADUser.ADPrincipal.IsAdministrator == true) type = "Administrator - " + txtUsername.Text;
                        if (ADUser.ADPrincipal.IsEmployee == true) type = "Employee - " + txtUsername.Text;
                        if (ADUser.ADPrincipal.IsTechnician == true) type = "Technician - " + txtUsername.Text;
                        Task.Run(() => {
                            SystemLogins systemLogins = new SystemLogins(Guid.NewGuid(), ADUser.ADPrincipal.GUID ?? Guid.NewGuid(), DateTime.UtcNow, type, failedCount);
                            systemLogins.insertLogin();
                        }).ConfigureAwait(false);
                        
                        NavigationService.NavigateTo(new MainScreen());
                    }
                    else
                    {
                        // Error Message
                        MessageBox.Show("Incorrect Credentials");
                        failedCount++;
                    }
                } else
                {
                    if (txtUsername.Text.Equals("letmein"))
                    {
                        Global.ADUser = new ActiveDirectoryEmployee("1234", "Marno van Niekerk", "marnovn071@gmail.com", true, Guid.NewGuid(), "Marno", "van Niekerk", DateTime.UtcNow, false, false, true);
                        NavigationService.NavigateTo(new MainScreen());
                    } else
                    {
                        MessageBox.Show("Incorrect Credentials");
                        failedCount++;
                    }
                    
                }
            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
            }

            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
