using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    class Calls
    {

        #region Field
        private Guid guid;
        private string callMemo;
        private int duration;
        private string crsId;
        private Guid employeeGuid;
        private Guid clientGuid;
        #endregion


        #region Properties
        public Calls()
        {

        }

        public Calls(Guid guid, string callMemo, int duration, string crsId, Guid employeeGuid, Guid clientGuid)
        {

        }

        #endregion

        #region Properties

        public Guid ClientGUID
        {
            get { return clientGuid; }
            set { clientGuid = value; }
        }


        public Guid EmployeeGUID
        {
            get { return employeeGuid; }
            set { employeeGuid = value; }
        }


        public string CrsID
        {
            get { return crsId; }
            set { crsId = value; }
        }


        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }


        public string CallMemo
        {
            get { return callMemo; }
            set { callMemo = value; }
        }


        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Calls objtoo = obj as Calls;
            if ((object)objtoo == null)
            {
                return false;
            }

            return (this.Guid == objtoo.Guid);
        }

        public override int GetHashCode()
        {
            return this.Guid.GetHashCode();
        } 
        #endregion

    }
}
