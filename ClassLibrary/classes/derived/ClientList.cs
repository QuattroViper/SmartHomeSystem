using ClassLibrary.classes.derived;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes.derived
{
    public class ClientList : Client
    {

        private List<Client> clientList;

        public ClientList()
        {
            this.clientList = getAllClients();
        }

        public ClientList(Guid guid, string name, string surname, AccountGuid accountGuid) : base(guid, name, surname, accountGuid)
        {
            this.Name = name;
            this.GUID = guid;
            this.Surname = surname;
            this.Account = accountGuid;
        }

        public List<Client> Clients
        {
            get { return clientList; }
            set { clientList = value; }
        }

        List<Client> getAllClients()
        {
            List<Client> clients = new List<Client>();


            DataTable dataList = DatabaseHandler.getInstance().getFromStringQuery("SELECT guid, name, surname,account_guid FROM tblClient");

            foreach (DataRow row in dataList.Rows)
            {
                clients.Add(new Client(new Guid(row["guid"].ToString()), (string)row["name"], (string)row["surname"],new AccountGuid(new Guid(row["account_guid"].ToString())) ));
            }


            return clients;
        }


    }
}
