using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public class CallInformation
    {


        #region Fields
        private Guid guid;
        private string callMemo;
        private double duration;
        private string crsID;
        private Guid employeeGuid;
        private Guid clientGuid;
        #endregion



        #region Constructors
        public CallInformation()
        {

        }

        public CallInformation(Guid guid, string callMemo, double duration, string crsID, Guid employeeGuid, Guid clientGuid)
        {
            this.guid = guid;
            this.callMemo = callMemo;
            this.duration = duration;
            this.crsID = crsID;
            this.employeeGuid = employeeGuid;
            this.clientGuid = clientGuid;
        }

        #endregion

        #region Properties


        public string TableName
        {
            get { return "dbo.tblCalls"; }
        }

        public SortedDictionary<string, string> TableDataInfo
        {
            get
            {
                return new SortedDictionary<string, string>() {
                    { "guid", "GUID"},
                    { "call_memo", "CallMemo" },
                    { "duration", "Duration" },
                    { "crs_id", "CRSID" },
                    { "employee_guid", "EmployeeGUID" },
                    { "client_guid", "ClientGUID" },
                };
            }
        }

        public Guid EmployeeGUID
        {
            get { return employeeGuid; }
            set { employeeGuid = value; }
        }


        public Guid ClientGUID
        {
            get { return clientGuid; }
            set { clientGuid = value; }
        }


        public string CRSID
        {
            get { return crsID; }
            set { crsID = value; }
        }


        public double Duration
        {
            get { return duration; }
            set { duration = value; }
        }


        public string CallMemo
        {
            get { return callMemo; }
            set { callMemo = value; }
        }


        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }
        #endregion

        #region Methods
        public void insertCallToDatabase()
        {
            DataHandler datahandler = new DataHandler(new DataHandlerHelper());

            datahandler.dynamicInsertQuery<CallInformation>(this);

        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            CallInformation objtoo = obj as CallInformation;
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
        #endregion

    }
}
