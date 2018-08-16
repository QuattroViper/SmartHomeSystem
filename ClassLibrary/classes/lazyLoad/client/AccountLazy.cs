using ClassLibrary.functions;
using DataAccess;
using ErrorHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes.lazyLoad.client
{
    public class AccountLazy : Account, ILazyLoad<AccountLazy>
    {

        private object locker = new object();
        private AccountLazy accountLazyL;


        public AccountLazy(Guid guid)
        {
            this.accountLazyL = lazyLoad(guid);
            this.GUID = guid;
            this.AccountType = accountLazyL.AccountType;
            this.CostPerMonth = accountLazyL.CostPerMonth;
            this.BillsDirectory = accountLazyL.BillsDirectory;
            this.IsLate = accountLazyL.IsLate;
            this.Credit = accountLazyL.Credit;
            this.RegisteredOn = accountLazyL.RegisteredOn;
            this.Card = accountLazyL.Card;
        }

        AccountLazy(Guid guid, string accountType, decimal costPerMonth, Dictionary<string,string> bills, bool isLate, decimal credit, DateTime registeredOn, Card card ) : base(guid, accountType, costPerMonth, bills, isLate, credit, registeredOn, card)
        {
            this.GUID = guid;
            this.AccountType = accountType;
            this.CostPerMonth = costPerMonth;
            this.BillsDirectory = bills;
            this.IsLate = isLate;
            this.Credit = credit;
            this.RegisteredOn = registeredOn;
            this.Card = card;
        }

        public AccountLazy()
        {
                
        }

        public AccountLazy AccountLazyL
        {
            get { return accountLazyL; }
            set { accountLazyL = value; }
        }

        private AccountLazy DefaultAccountL
        {
            get { return new AccountLazy(); }
        }

        public AccountLazy lazyLoad(Guid guid)
        {
            AccountLazy accountL;

            try
            {

                lock (locker)
                {
                    string queryBills = string.Format(QueryStrings.getAllBillsOnClientGuid, guid);
                    DataTable bills = DatabaseHandler.getInstance().getFromStringQuery(queryBills);

                    Dictionary<string, string> directories = new Dictionary<string, string>();

                    if (bills.Rows.Count != 0)
                    {
                        foreach (DataRow row in bills.Rows)
                        {
                            directories.Add((string)row["bill_name"], (string)row["bill_directory"]);
                        }

                    }

                    string query = string.Format(QueryStrings.getClientAccountOnGuid, guid);

                    DataTable accountData = DatabaseHandler.getInstance().getFromStringQuery(query);

                    if (accountData.Rows.Count == 1)
                    {
                        Card card = new Card((string)accountData.Rows[0]["card_object_encrypted_string"]).decryptString();

                        accountL = new AccountLazy(new Guid(accountData.Rows[0]["guid"].ToString()), (string)accountData.Rows[0]["accountType"], Convert.ToDecimal(accountData.Rows[0]["costperMonth"]), directories, Convert.ToBoolean(accountData.Rows[0]["is_late"]), Convert.ToDecimal(accountData.Rows[0]["credit"]), Convert.ToDateTime(accountData.Rows[0]["registered_on"].ToString()), card);
                    }
                    else
                    {
                        accountL = DefaultAccountL;
                    }
                }


            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
                accountL = DefaultAccountL;
            }

            return accountL;
        }
    }
}
