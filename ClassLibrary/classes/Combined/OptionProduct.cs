using ClassLibrary.functions;
using DataAccess;
using ErrorHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes.Combined
{
    public class OptionProduct
    {
        private object locker = new object();

        private Guid guid;
        private Guid optionGuid;
        private Guid productGuid;

        public OptionProduct()
        {

        }

        public OptionProduct(Guid optionGuid, Guid productGuid)
        {
            this.optionGuid = optionGuid;
            this.productGuid = productGuid;
            this.guid = Guid.NewGuid();
        }

        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public Guid ProductGuid
        {
            get { return productGuid; }
            set { productGuid = value; }
        }


        public Guid OptionGuid
        {
            get { return optionGuid; }
            set { optionGuid = value; }
        }

        public string TableName
        {
            get { return "dbo.tblProductOption"; }
        }


        public SortedDictionary<string, string> TableDataInfo
        {
            get
            {
                return new SortedDictionary<string, string>() {
                    { "guid", "GUID"},
                    { "product_guid", "ProductGuid" },
                    { "option_guid", "OptionGuid" },
                };
            }
        }

        public void InsertProductToOption()
        {
            DataHandler datahandler = new DataHandler(new DataHandlerHelper());
            datahandler.dynamicInsertQuery<OptionProduct>(this);
        }

        public void removeProductFromOption()
        {
            this.guid = getGuidFromOptionAndProduct();
            DataHandler datahandler = new DataHandler(new DataHandlerHelper());
            datahandler.dynamicDeleteQuery<OptionProduct>(this);
        }

        Guid getGuidFromOptionAndProduct()
        {
            Guid guid = new Guid();

            try
            {

                lock (locker)
                {
                    string query = string.Format(QueryStrings.OptionProduct.getGuidFromOptionAndProduct, this.optionGuid, this.productGuid);

                    DataTable systemData = DatabaseHandler.getInstance().getFromStringQuery(query);

                    if (systemData.Rows.Count == 1)
                    {
                        DataRow row = systemData.Rows[0];
                        return new Guid(row["guid"].ToString());

                    }
                    else
                    {
                        ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                        error.handle(new Exception("No Rows"), true);
                        
                    }
                }

            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
                
            }

            return guid;
        }

    }
}
