using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    class CustomAttributes : Attribute
    {
        

    }
     
    public class databaseMap : Attribute
    {
        private string dbColumn;

        public databaseMap(string dbColumn)
        {
            this.dbColumn = dbColumn;
        }

        public string DBColumn
        {
            get { return dbColumn; }
            set { dbColumn = value; }
        }
    }

}
