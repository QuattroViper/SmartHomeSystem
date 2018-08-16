using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes.derived
{
    public class AccountGuid : Account
    {

        public AccountGuid(Guid guid) : base()
        {
            this.GUID = guid;
        }

    }
}
