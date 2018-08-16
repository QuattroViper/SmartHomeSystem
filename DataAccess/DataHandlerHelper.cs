using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataHandlerHelper
    {

        public enum Connection {
            ConnectionOne = 0,
            ConnecitonTwo = 1
        }

        private string connectionStringOne;
        private string connectionStringTwo;
        private string providerName;
        private Connection connectionToUse;


        //public DataHandlerHelper()
        //{

        //}

        public DataHandlerHelper(
            string connectionStringOne = "Data Source=192.168.2.222;Initial Catalog=SmartHomeSystem;User ID=applicationLogin;Password=BelgiumCampus123.", 
            string connectionStringTwo = "Data Source=41.168.11.34;Initial Catalog=SmartHomeSystem;User ID=applicationLogin;Password=BelgiumCampus123.", 
            string providerName = "System.Data.SqlClient",
            Connection connectionToUse = Connection.ConnectionOne
            )
        {
            this.connectionStringOne = connectionStringOne;
            this.connectionStringTwo = connectionStringTwo;
            this.providerName = providerName;
            this.connectionToUse = connectionToUse;
        }

        public Connection ConnectionToUse
        {
            get { return connectionToUse; }
            set { connectionToUse = value; }
        }

        public string ProviderName
        {
            get { return providerName; }
            set { providerName = value; }
        }


        public string ConnectionStringTwo
        {
            get { return connectionStringTwo; }
            set { connectionStringTwo = value; }
        }


        public string ConnectionStringOne
        {
            get { return connectionStringOne; }
            set { connectionStringOne = value; }
        }


    }
}
