using ClassLibrary.functions;
using DataAccess;
using ErrorHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes.lazyLoad.client
{
    public class ProductLazy : Product, ILazyLoad<List<Product>>
    {

        private object locker = new object();
        private List<Product> productList;

        public ProductLazy()
        {
                
        }

        public ProductLazy(Guid guid)
        {
            this.productList = lazyLoad(guid);
        }

        ProductLazy(Guid guid, string productCode, string name, string description, decimal price, string type, int warrentyPeriod, bool inStock, DateTime arrivalDate, string manufacturer, string model, string serialNumber) 
            : base(guid, productCode, name, description, price, type, warrentyPeriod, inStock, arrivalDate, manufacturer, model, serialNumber)
        {

        }

        List<Product> DefaultProduct
        {
            get { return new List<Product>(); }
        }

        public List<Product> ProductList
        {
            get { return productList; }
            set { productList = value; }
        }


        public List<Product> lazyLoad(Guid guid)
        {
            List<Product> productstList = new List<Product>();
											
            try
            {

                lock (locker)
                {
                    string query = string.Format(QueryStrings.getProductsBasedOnOptionGuid, guid);

                    DataTable systemData = DatabaseHandler.getInstance().getFromStringQuery(query);

                    if (systemData.Rows.Count != 0)
                    {
                        foreach (DataRow row in systemData.Rows)
                        {
                            productstList.Add(
                                new Product(
                                    new Guid(row["guid"].ToString()), (string)row["product_code"], (string)row["name"], (string)row["description"], Convert.ToDecimal(row["price"]), (string)row["type"], Convert.ToInt32(row["warrenty_period"]), Convert.ToBoolean(row["in_stock"]), Convert.ToDateTime(row["arrival_date"]), (string)row["manufacturer"], (string)row["model"], (string)row["serial_number"]
                                    )
                                );
                        }
                    }
                    else
                    {
                        return DefaultProduct;
                    }
                }


            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
                productstList = DefaultProduct;
            }
            return productstList;
        }

        public List<Product> loadAllProducts()
        {
            List<Product> productstList = new List<Product>();

            try
            {

                lock (locker)
                {
                    string query = string.Format(QueryStrings.getAllProducts);

                    DataTable systemData = DatabaseHandler.getInstance().getFromStringQuery(query);

                    if (systemData.Rows.Count != 0)
                    {
                        foreach (DataRow row in systemData.Rows)
                        {
                            productstList.Add(
                                new Product(
                                    new Guid(row["guid"].ToString()), (string)row["product_code"], (string)row["name"], (string)row["description"], Convert.ToDecimal(row["price"]), (string)row["type"], Convert.ToInt32(row["warrenty_period"]), Convert.ToBoolean(row["in_stock"]), Convert.ToDateTime(row["arrival_date"]), (string)row["manufacturer"], (string)row["model"], (string)row["serial_number"]
                                    )
                                );
                        }
                    }
                    else
                    {
                        return DefaultProduct;
                    }
                }


            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
                productstList = DefaultProduct;
            }
            return productstList;
        }
    }
}
