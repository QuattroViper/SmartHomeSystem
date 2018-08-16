using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public class System
    {
        private Guid guid;
        List<Option> options;
        Status status;
        Contract contract;

        public System()
        {

        }

        public System(Guid guid, List<Option> options, Status status, Contract contract)
        {
            this.guid = guid;
            this.options = options;
            this.status = status;
            this.contract = contract;
        }

        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public Contract Contract
        {
            get { return contract; }
            set { contract = value; }
        }

        public List<Option> Options
        {
            get { return options; }
            set { options = value; }
        }

        public Status Status
        {
            get { return status; }
            set { status = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            System objtoo = obj as System;
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
    }
}
