using ClassLibrary.classes.lazyLoad.client;
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

namespace SmartHomeSystem.fragments.ClientsFrags
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window, IDetachContent, INotifyClientChange
    {
        public Account()
        {
            InitializeComponent();
        }

        public Account(Guid guid)
        {
            InitializeComponent();

            updateView(guid);
        }

        public void DetachContent()
        {
            RemoveLogicalChild(Content);
        }

        public void updateView(Guid guid)
        {
            AccountLazy accountLazy = new AccountLazy(guid);
            txtCostPerMonth.Text = string.Format("R {0:0.00}", accountLazy.CostPerMonth).EndsWith("00") ? string.Format("R {0}", Convert.ToInt32(accountLazy.CostPerMonth)) : string.Format("R {0:0.00}", accountLazy.CostPerMonth);
            txtIsLate.Text = accountLazy.IsLate ? "Account is Late" : "Account up to date" ;
            txtCredit.Text = string.Format("R {0:0.00}", accountLazy.Credit).EndsWith("00") ? string.Format("R {0}", Convert.ToInt32(accountLazy.Credit))  : string.Format("R {0:0.00}", accountLazy.Credit);
            txtDate.Text = accountLazy.RegisteredOn.ToString("d MMMM, yyyy");
            txtAccountType.Text = accountLazy.AccountType;

            txtCardBank.Content = accountLazy.Card.Bank;
            txtCardNumber.Content = accountLazy.Card.CardNumber;
            txtCardHolderName.Content = accountLazy.Card.CardHolder;
            txtCardDate.Content = accountLazy.Card.ExpireDate.ToString("dd/MM");
        }
    }
}
