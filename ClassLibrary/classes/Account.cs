using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public partial class Account
    {

        #region Fields

        private Guid guid;
        private string accountType;
        private decimal costPerMonth;
        private Dictionary<string,string> billsDirectory;
        private bool isLate;
        private decimal credit;
        private DateTime registeredOn;
        private Card card;

        #endregion

        #region Constructors

        public Account()
        {

        }

        /// <summary>
        /// Constructor where all of the fields are available to use. 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="accountType"></param>
        /// <param name="costPerMonth"></param>
        /// <param name="billsDirectory"></param>
        /// <param name="isLate"></param>
        /// <param name="credit"></param>
        /// <param name="registeredOn"></param>
        /// <param name="card"></param>
        public Account(Guid guid, string accountType, decimal costPerMonth, Dictionary<string, string> billsDirectory, bool isLate, decimal credit, DateTime registeredOn, Card card)
        {
            this.guid = guid;
            this.accountType = accountType;
            this.costPerMonth = costPerMonth;
            this.billsDirectory = billsDirectory;
            this.isLate = isLate;
            this.credit = credit;
            this.registeredOn = registeredOn;
            this.card = card.decryptString();
        }

        #endregion

        #region Properties

        public Card Card
        {
            get { return card; }
            set { card = value; }
        }

        public DateTime RegisteredOn
        {
            get { return registeredOn; }
            set { registeredOn = value; }
        }


        public decimal Credit
        {
            get { return credit; }
            set { credit = value; }
        }

        public bool IsLate
        {
            get { return isLate; }
            set { isLate = value; }
        }


        public Dictionary<string, string> BillsDirectory
        {
            get { return billsDirectory; }
            set { billsDirectory = value; }
        }


        public decimal CostPerMonth
        {
            get { return costPerMonth; }
            set { costPerMonth = value; }
        }


        public string AccountType
        {
            get { return accountType; }
            set { accountType = value; }
        }


        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        #endregion

    }

    public partial class Account
    {

        #region Methods
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Account objtoo = obj as Account;
            if ((object)objtoo == null)
            {
                return false;
            }

            return (this.GUID == objtoo.GUID);
        }

        public override int GetHashCode()
        {
            return this.GUID.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public void generateBill()
        {

        }

        public bool sendBill()
        {
            bool sent = false;

            return sent;
        }

        public void deleteBillAfterTwoYears()
        {

        }

        public void sendLateNotification()
        {

        }

        #endregion
    }
}
