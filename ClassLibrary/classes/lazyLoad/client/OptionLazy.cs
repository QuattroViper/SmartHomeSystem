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
    public class OptionLazy : Option, ILazyLoad<List<Option>>
    {

        private object locker = new object();
        private List<Option> optionList;
        private decimal combinedPrice;




        public OptionLazy()
        {

        }

        public OptionLazy(Guid guid)
        {
            this.optionList = lazyLoad(guid);
        }

        OptionLazy(Guid guid, string name, string description, List<GeoData> recommendedFor, List<Product> products, List<string> functions, decimal price, int installationTime, decimal installByCompanyCost, decimal combinedPrice) 
            : base(guid, name, description, recommendedFor, products, functions, price, installationTime, installByCompanyCost)
        {
            this.combinedPrice = combinedPrice;
        }

        public List<Option> OptionList
        {
            get { return this.optionList; }
            set { this.optionList = value; }
        }

        public decimal CombinedPrice
        {
            get { return combinedPrice; }
            set { combinedPrice = value; }
        }

        List<Option> DefaultOptionList
        {
            get { return new List<Option>(); }
        }

        public List<Option> lazyLoad(Guid guid)
        {
            List<Option> optionsList = new List<Option>();

            try
            {

                lock (locker)
                {
                    string query = string.Format(QueryStrings.getOptionsBasedOnSystemtGuid, guid);

                    DataTable systemData = DatabaseHandler.getInstance().getFromStringQuery(query);

                    if (systemData.Rows.Count != 0)
                    {
                        foreach (DataRow row in systemData.Rows)
                        {
                            optionsList.Add(
                                    new OptionLazy(
                                            new Guid(row["option_guid"].ToString()), (string)row["name"], (string)row["description"], new List<GeoData>(), new List<Product>(), LoadFunctionsForOption(new Guid(row["option_guid"].ToString())), Convert.ToDecimal(row["price"]), Convert.ToInt32(row["installation_time"]), Convert.ToDecimal(row["install_by_company_cost"]), Convert.ToDecimal(row["combined_price"])
                                        )
                                );
                        }
                    }
                    else
                    {

                    }
                }

            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
                optionsList = DefaultOptionList;
            }
            return optionsList;
        }

        List<string> LoadFunctionsForOption(Guid guid)
        {
            List<string> functionsList = new List<string>();

            try
            {

                string query = string.Format(QueryStrings.getFunctionsBasedOnOptionGuid, guid);

                DataTable systemData = DatabaseHandler.getInstance().getFromStringQuery(query);

                if (systemData.Rows.Count != 0)
                {
                    foreach (DataRow row in systemData.Rows)
                    {
                        functionsList.Add(row["function"].ToString());
                    }
                }
                else
                {
                    return new List<string>();
                }


            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
                functionsList = new List<string>();
            }
            return functionsList;


        }

        public List<OptionLazy> LoadAllOptions()
        {
            List<OptionLazy> functionsList = new List<OptionLazy>();

            try
            {

                string query = string.Format(QueryStrings.Option.getAllOptions);

                DataTable systemData = DatabaseHandler.getInstance().getFromStringQuery(query);

                if (systemData.Rows.Count != 0)
                {
                    foreach (DataRow row in systemData.Rows)
                    {
                        functionsList.Add(
                                new OptionLazy(
                                    new Guid(row["guid"].ToString()), (string)row["name"], (string)row["description"], new List<GeoData>(), new List<Product>(), LoadFunctionsForOption(new Guid(row["guid"].ToString())), Convert.ToDecimal(row["price"]), Convert.ToInt32(row["installation_time"]), Convert.ToDecimal(row["install_by_company_cost"]), Convert.ToDecimal(row["combined_price"])
                                )
                            );
                    }
                }
                else
                {
                    return new List<OptionLazy>();
                }


            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
                functionsList = new List<OptionLazy>();
            }
            return functionsList;


        }


        public OptionLazy getSingleOption (Guid guid)
        {
            OptionLazy opt = null;


            try
            {

                lock (locker)
                {
                    string query = string.Format(QueryStrings.Option.getSingleOptionOnGuid, guid);

                    DataTable systemData = DatabaseHandler.getInstance().getFromStringQuery(query);

                    if (systemData.Rows.Count == 1)
                    {
                        DataRow row = systemData.Rows[0];
                        opt = new OptionLazy(
                             new Guid(row["option_guid"].ToString()), (string)row["name"], (string)row["description"], new List<GeoData>(), new List<Product>(), LoadFunctionsForOption(new Guid(row["option_guid"].ToString())), Convert.ToDecimal(row["price"]), Convert.ToInt32(row["installation_time"]), Convert.ToDecimal(row["install_by_company_cost"]), Convert.ToDecimal(row["combined_price"])
                        );


                    }
                    else
                    {
                        ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                        error.handle(new Exception("No Rows"), true);
                        opt = new OptionLazy();
                    }
                }

            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
                opt = new OptionLazy();
            }

            return opt;
        }

    }
}
