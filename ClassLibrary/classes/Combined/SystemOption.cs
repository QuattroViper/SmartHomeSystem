using ClassLibrary.functions;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes.Combined
{
    public class SystemOption
    {

        object locker = new object();

        private Guid guid;
        private Guid systemGuid;
        private Guid optionGuid;

        public SystemOption()
        {

        }

        public SystemOption(Guid systemGuid, Guid optionGuid)
        {
            this.guid = Guid.NewGuid();
            this.systemGuid = systemGuid;
            this.optionGuid = optionGuid;
        }

        public Guid OptionGuid
        {
            get { return optionGuid; }
            set { optionGuid = value; }
        }


        public Guid SystemGuid
        {
            get { return systemGuid; }
            set { systemGuid = value; }
        }


        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public string TableName
        {
            get { return "dbo.tblSystemOption"; }
        }


        public SortedDictionary<string, string> TableDataInfo
        {
            get
            {
                return new SortedDictionary<string, string>() {
                    { "guid", "GUID"},
                    { "system_guid", "SystemGuid" },
                    { "option_guid", "OptionGuid" },
                };
            }
        }

        public void insertIntoSystemOption()
        {
            DataHandler datahandler = new DataHandler(new DataHandlerHelper());
            datahandler.dynamicInsertQuery<SystemOption>(this);
        }

        public void removeFromClientOptions()
        {
            string query = string.Format(QueryStrings.Option.deleteOptionFromSystem, this.optionGuid);

            DataTable systemData = DatabaseHandler.getInstance().getFromStringQuery(query);
        }


    }
}
