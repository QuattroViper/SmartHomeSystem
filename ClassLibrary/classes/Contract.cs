using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public class Contract
    {

        private Guid guid;
        private bool isActive;
        private string contractIdentifier;
        private string serviceLevel;
        private DateTime startDate;
        private DateTime endDate;
        private string upgradeOptions;

        public Contract()
        {

        }

        public Contract(Guid guid, bool isActive, string contractIdentifier, string serviceLevel, DateTime startDate, DateTime endDate, string upgradeOptions)
        {
            this.guid = guid;
            this.isActive = isActive;
            this.contractIdentifier = contractIdentifier;
            this.serviceLevel = serviceLevel;
            this.startDate = startDate;
            this.endDate = endDate;
            this.upgradeOptions = upgradeOptions;
        }

        public string UpgradeOptions
        {
            get { return upgradeOptions; }
            set { upgradeOptions = value; }
        }


        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }


        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }


        public string ServiceLevel
        {
            get { return serviceLevel; }
            set { serviceLevel = value; }
        }


        public string ContractIdentifier
        {
            get { return contractIdentifier; }
            set { contractIdentifier = value; }
        }


        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }




        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Contract objtoo = obj as Contract;
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


    }
}
