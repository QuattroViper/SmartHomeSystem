using ClassLibrary.functions;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public class SystemLogins
    {
        private Guid guid;
        private Guid loginGuidUsed;
        private DateTime loginDate;
        private string type;
        private int failedAttempts;

        public SystemLogins()
        {

        }

        public string TableName
        {
            get { return "dbo.tblSystemLogins"; }
        }

        public SortedDictionary<string, string> TableDataInfo
        {
            get
            {
                return new SortedDictionary<string, string>() {
                    { "guid", "GUID"},
                    { "login_guid_used", "LoginGuidUsed" },
                    { "login_date", "LoginDate" },
                    { "login_time", "Time" },
                    { "type", "Type" },
                    { "failed_attemps", "FailedAttempts" },
                };
            }
        }

        public SystemLogins(Guid guid,Guid loginGuidUsed, DateTime loginDate, string type, int failedAttempts)
        {
            this.guid = guid;
            this.loginGuidUsed = loginGuidUsed;
            this.loginDate = loginDate;
            this.type = type;
            this.failedAttempts = failedAttempts;
        }

        public int FailedAttempts
        {
            get { return failedAttempts; }
            set { failedAttempts = value; }
        }

        public TimeSpan Time
        {
            get { return LoginDate.TimeOfDay; }
        }


        public string Type
        {
            get { return type; }
            set { type = value; }
        }


        public DateTime LoginDate
        {
            get { return loginDate; }
            set { loginDate = value; }
        }


        public Guid LoginGuidUsed
        {
            get { return loginGuidUsed; }
            set { loginGuidUsed = value; }
        }


        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public void insertLogin()
        {
            DataHandler datahandler = new DataHandler(new DataHandlerHelper());

            datahandler.dynamicInsertQuery<SystemLogins>(this);
        }

        public List<SystemLogins> getAllLogings()
        {
            List<SystemLogins> allLogings = new List<SystemLogins>();

            string query = QueryStrings.Logins.getAllLogings;

            DataTable data = DatabaseHandler.getInstance().getFromStringQuery(query);

            foreach (DataRow row in data.Rows)
            {
                allLogings.Add(
                        new SystemLogins(new Guid(row["guid"].ToString()),new Guid(row["login_guid_used"].ToString()), Convert.ToDateTime(row["login_date"].ToString()).Date.Add(Convert.ToDateTime(row["login_time"].ToString()).TimeOfDay), (string)row["type"], Convert.ToInt32(row["failed_attemps"]))
                    );
            }

            return allLogings;

        }

    }
}
