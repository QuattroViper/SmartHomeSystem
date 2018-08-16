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
    public class ClientLazy : Client, ILazyLoadAll<List<Client>>
    {

        private object locker = new object();
        private List<Client> clientList;
        private Guid accountGuid;
        private Guid loginGuid;
        private Guid clientGuid;




        public ClientLazy()
        {
            this.clientList = lazyLoad();
        }

        ClientLazy(Guid guid, Guid accountGuid, Guid loginGuid, string clientIdentifier, string id, string name, string surname, DateTime dateOfBirth, string gender) 
            : base(guid, clientIdentifier, id, name, surname, dateOfBirth, gender)
        {
            this.accountGuid = accountGuid;
            this.loginGuid = loginGuid;
            this.clientGuid = guid;
        }

        public Guid ClientGuid
        {
            get { return clientGuid; }
            set { clientGuid = value; }
        }


        public Guid LoginGuid
        {
            get { return loginGuid; }
            set { loginGuid = value; }
        }

        public Guid AccountGuid
        {
            get { return accountGuid; }
            set { accountGuid = value; }
        }

        public List<Client> DefaultClientList
        {
            get { return new List<Client>(); }
        }

        public List<Client> ClientList
        {
            get { return clientList; }
            set { clientList = value; }
        }

        public List<Client> lazyLoad()
        {
            List<Client> cList = new List<Client>();

            try
            {
                //						

                lock (locker)
                {
                    string query = string.Format(QueryStrings.Client.getAllClients);

                    DataTable clientData = DatabaseHandler.getInstance().getFromStringQuery(query);

                    if (clientData.Rows.Count != 0)
                    {
                        foreach (DataRow row in clientData.Rows)
                        {

                            Dictionary<string, object> idDecoded = new Dictionary<string, object>();
                            idDecoded = functions.functions.decodeID((string)row["identification"]);

                            cList.Add(
                                    new ClientLazy(new Guid(row["client_guid"].ToString()), new Guid(row["account_guid"].ToString()), new Guid(row["login_guid"].ToString()), (string)row["client_identifier"], (string)row["identification"], (string)row["name"], (string)row["surname"], (DateTime)idDecoded["date"], (string)idDecoded["gender"])
                                );
                        }
                    }
                    else
                    {
                        cList = DefaultClientList;
                    }
                }

            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
                cList = DefaultClientList;
            }

            return cList;
        }
    }
}
