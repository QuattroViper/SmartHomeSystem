using ClassLibrary.classes.derived;
using ClassLibrary.classes.validation;
using ClassLibrary.functions;
using ClassLibrary.interfaces;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClassLibrary.classes
{
    public class Client : Person
    {

        #region Fields
        private Guid guid;
        private List<System> systems;
        private Account account;
        private List<Address> addresses;
        private Login login;
        private Contact contactDetails;
        private string clientIdentifier;
        #endregion


        #region Constuctors
        public Client()
        {

        }

        public Client(Guid guid, string name, string surname) : base(guid, name, surname)
        {

        }

        public Client(Guid guid, string id, string name, string surname, string clientIdentifier) : base(guid, id, name, surname)
        {
            this.clientIdentifier = clientIdentifier;
        }




        public Client(Guid guid, List<System> systems, Account account, List<Address> addresses, Login login, Contact contactDetails)
        {
            this.guid = guid;
            this.systems = systems;
            this.account = account;
            this.addresses = addresses;
            this.login = login.decryptString();
            this.contactDetails = contactDetails;
        }

        public Client(Guid guid, List<System> systems, Account account, List<Address> addresses, Login login, Contact contactDetails, string clientIdentifier, string id, string name, string surname, DateTime dateOfBirth, string gender) 
            : base(guid, id, name,  surname,  dateOfBirth,  gender)
        {
            this.guid = guid;
            this.systems = systems;
            this.account = account;
            this.addresses = addresses;
            this.login = login.decryptString();
            this.clientIdentifier = clientIdentifier;
            this.contactDetails = contactDetails;
        }

        public Client(Guid guid, string clientIdentifier, string id, string name, string surname, DateTime dateOfBirth, string gender)
            : base(guid, id, name, surname, dateOfBirth, gender)
        {
            this.guid = guid;
            this.clientIdentifier = clientIdentifier;
        }

        public Client(Guid guid, List<Address> addresses, Login login, Contact contactDetails, string clientIdentifier, string id, string name, string surname, DateTime dateOfBirth, string gender)
            : base(guid, id, name, surname, dateOfBirth, gender)
        {
            this.guid = guid;
            this.systems = new List<System>();
            this.account = new Account();
            this.addresses = addresses;
            this.login = login.decryptString();
            this.clientIdentifier = clientIdentifier;
            this.contactDetails = contactDetails;
        }

        public Client(Guid guid, string name, string surname, AccountGuid accountGuid)
        {
            this.Name = name;
            this.guid = guid;
            this.Surname = surname;
            this.account = accountGuid;
        }

        #endregion

        #region Properties


        public Contact ContactDetails
        {
            get { return contactDetails; }
            set { contactDetails = value; }
        }

        public Account Account
        {
            get { return account; }
            set { account = value; }
        }

        public string ClientIdetifier
        {
            get { return clientIdentifier; }
            set { clientIdentifier = value; }
        }

        public List<Address> Addresses
        {
            get { return addresses; }
            set { addresses = value; }
        }

        public List<System> Systems
        {
            get { return systems; }
            set { systems = value; }
        }

        public new Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public Login Login
        {
            get { return login; }
            set { login = value; }
        }

        #endregion

        #region Methods


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Client objtoo = obj as Client;
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

        public ValidationResult validateInputs<T>(object target)
        {
            OptionValidation<T> validatation = new OptionValidation<T>();
            return validatation.validate<T>(target);
        }

        public bool insertClient()
        {

            if (systems.Count != 1)
                throw new Exception();

            if (addresses.Count != 1)
                throw new Exception();

            Address addressTemp = this.addresses.ElementAt(0);
            System system = this.systems.ElementAt(0);

            Guid systemGUID = Guid.NewGuid();
            Guid contractGUID = Guid.NewGuid();
            Guid clientGUID = Guid.NewGuid();
            Guid accountGUID = Guid.NewGuid();
            Guid loginGUID = Guid.NewGuid();
            Guid contactGUID = Guid.NewGuid();
            Guid AddressGUID = Guid.NewGuid();


            Dictionary<string, KeyValuePair<string, string>> parameters = new Dictionary<string, KeyValuePair<string, string>>()
            {
                { QueryBuilder.ClientInsertQuery.ClientGUID, new KeyValuePair<string,string>(clientGUID.ToString(), "GUID") },
                { QueryBuilder.ClientInsertQuery.AccountGUID, new KeyValuePair<string,string>(accountGUID.ToString(), "GUID")},
                { QueryBuilder.ClientInsertQuery.AccountType, new KeyValuePair<string,string>(this.account.AccountType.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.CostPerMonth, new KeyValuePair<string,string>(this.account.CostPerMonth.ToString(), "MONEY") },
                { QueryBuilder.ClientInsertQuery.IsLate, new KeyValuePair<string,string>(this.account.IsLate.ToString(), "BOOL") },
                { QueryBuilder.ClientInsertQuery.Credit, new KeyValuePair<string,string>(this.account.Credit.ToString(), "MONEY") },
                { QueryBuilder.ClientInsertQuery.RegisteredOn, new KeyValuePair<string,string>(this.account.RegisteredOn.ToString(), "DATETIME") },
                { QueryBuilder.ClientInsertQuery.CardEncryptedString, new KeyValuePair<string,string>(this.account.Card.EncryptedString.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.Name, new KeyValuePair<string,string>(this.Name.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.Surname, new KeyValuePair<string,string>(this.Surname.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.Identification, new KeyValuePair<string,string>(this.ID.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.LoginGUID, new KeyValuePair<string,string>(loginGUID.ToString(), "GUID") },
                { QueryBuilder.ClientInsertQuery.LoginEncryptedString, new KeyValuePair<string,string>(this.Login.EncryptedString.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.ContactGUID, new KeyValuePair<string,string>(contactGUID.ToString(), "GUID") },
                { QueryBuilder.ClientInsertQuery.ContactNumber, new KeyValuePair<string,string>(ContactDetails.ContactNumber.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.EmailAddress, new KeyValuePair<string,string>(ContactDetails.EmailAddress.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.AndroidDeviceID, new KeyValuePair<string,string>(ContactDetails.AndroidDeviceID.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.AppleDeviceID, new KeyValuePair<string,string>(ContactDetails.AppleDeviceID.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.AddressGuid, new KeyValuePair<string,string>(AddressGUID.ToString(), "GUID") },
                { QueryBuilder.ClientInsertQuery.Address1, new KeyValuePair<string,string>(addressTemp.Address1.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.Address2, new KeyValuePair<string,string>(addressTemp.Address2.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.Suburb, new KeyValuePair<string,string>(addressTemp.Suburb.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.PostalCode, new KeyValuePair<string,string>(addressTemp.PostalCode.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.City, new KeyValuePair<string,string>(addressTemp.City.ToString(), "STRING") },
                { QueryBuilder.ClientInsertQuery.Country, new KeyValuePair<string,string>(addressTemp.Country.ToString(), "STRING") },

            };

            bool status = DatabaseHandler.getInstance().nonQuery(QueryBuilder.ClientInsertQuery.sp, parameters);

            Dictionary<string, KeyValuePair<string, string>> parametersSystem = new Dictionary<string, KeyValuePair<string, string>>()
            {
                { QueryBuilder.SystemInsert.StatusGUID, new KeyValuePair<string,string>(Guid.NewGuid().ToString(), "GUID") },
                { QueryBuilder.SystemInsert.SystemGUID, new KeyValuePair<string,string>(systemGUID.ToString(), "GUID") },
                { QueryBuilder.SystemInsert.ContractGUID, new KeyValuePair<string,string>(contractGUID.ToString(), "GUID") },
                { QueryBuilder.SystemInsert.ClientGUID, new KeyValuePair<string,string>(clientGUID.ToString(), "GUID") },
                { QueryBuilder.SystemInsert.IsActive, new KeyValuePair<string,string>(system.Contract.IsActive.ToString(), "BOOL") },
                { QueryBuilder.SystemInsert.ContractIdentifier, new KeyValuePair<string,string>(system.Contract.ContractIdentifier.ToString(), "STRING") },
                { QueryBuilder.SystemInsert.ServiceLevel, new KeyValuePair<string,string>(system.Contract.ServiceLevel.ToString(), "STRING") },
                { QueryBuilder.SystemInsert.StartDate, new KeyValuePair<string,string>(system.Contract.StartDate.ToString(), "DATETIME") },
                { QueryBuilder.SystemInsert.EndDate, new KeyValuePair<string,string>(system.Contract.EndDate.ToString(), "DATETIME") },
                { QueryBuilder.SystemInsert.UpgradeOptions, new KeyValuePair<string,string>(system.Contract.UpgradeOptions.ToString(), "STRING") },
            };

            bool statusSystem = DatabaseHandler.getInstance().nonQuery(QueryBuilder.SystemInsert.sp, parametersSystem);

            return status && statusSystem;
        }


        public Client getClientOnAppointmentGuid(Guid guid)
        {
            string query = string.Format(QueryStrings.Client.getClientBasedOnAppointmentGuid, guid);

            DataTable data = DatabaseHandler.getInstance().getFromStringQuery(query);

            if (data.Rows.Count != 0)
            {
                DataRow row = data.Rows[0];
                return new Client(new Guid(row["guid"].ToString()), (string)row["identification"], (string)row["name"], (string)row["surname"], (string)row["client_identifier"]);

            }
            else
            {
                return new Client(new Guid(), "0000000000000", "None", "None", "A00000000");
            }

        }

        public Client getClientOnIdentifierOrID(string searcher)
        {
            List<Client> possibleClients = new List<Client>();

            string query = string.Format(QueryStrings.Client.getClientOnIdentifierOrID, searcher, searcher);

            DataTable data = DatabaseHandler.getInstance().getFromStringQuery(query);



            if (data.Rows.Count != 0)
            {
                DataRow row = data.Rows[0];
                return new Client(new Guid(row["guid"].ToString()), (string)row["name"], (string)row["surname"]);
            }


            return null;

        } 
        #endregion

    }
}
