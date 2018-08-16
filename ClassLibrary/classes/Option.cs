using ClassLibrary.classes.validation;
using ClassLibrary.functions;
using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClassLibrary.classes
{
    public partial class Option : INotifyPropertyChanged
    {
        private Guid guid;
        private string name;
        private string description;
        private List<GeoData> recommendedFor;
        private List<Product> products;
        private List<string> functions;
        private decimal price;
        private int installationTime;
        private decimal installByCompanyCost;

        public event PropertyChangedEventHandler PropertyChanged;

        public Option()
        {

        }

        public Option(Guid guid, string name, string description, List<GeoData> recommendedFor, List<Product> products, List<string> functions, decimal price, int installationTime, decimal installByCompanyCost) {
            this.guid = guid;
            this.name = name;
            this.description = description;
            this.recommendedFor = recommendedFor;
            this.products = products;
            this.functions = functions;
            this.price = price;
            this.installationTime = installationTime;
            this.installByCompanyCost = installByCompanyCost;
        }

        public string TableName
        {
            get { return "dbo.tblOption"; }
        }


        public SortedDictionary<string, string> TableDataInfo
        {
            get
            {
                return new SortedDictionary<string, string>() {
                    { "guid", "GUID"},
                    { "name", "Name" },
                    { "description", "Description" },
                    { "price", "Price" },
                    { "installation_time", "InstallationTime" },
                    { "install_by_company_cost", "InstallByCompanyCost" },
                    
                };
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public decimal InstallByCompanyCost
        {
            get { return installByCompanyCost; }
            set {
                installByCompanyCost = value;
                NotifyPropertyChanged();
            }
        }

        public string InstallByCompanyCostF
        {
            get { return string.Format("R {0:0.00}",installByCompanyCost); }
        }


        public int InstallationTime
        {
            get { return installationTime; }
            set {
                installationTime = value;
                NotifyPropertyChanged();
            }
        }

        public string InstallationTimeF
        {
            get { return string.Format("{0} Hours", installationTime); }
        }

        public string Name
        {
            get { return name; }
            set {
                name = value;
                NotifyPropertyChanged();
            }
        }

        public string Description
        {
            get { return description; }
            set {
                description = value;
                NotifyPropertyChanged();
            }
        }

        public List<Product> Products
        {
            get { return products; }
            set {
                products = value;
                NotifyPropertyChanged();
            }
        }

        public List<string> Functions
        {
            get { return functions; }
            set {
                functions = value;
                NotifyPropertyChanged();
            }
        }

        public decimal Price
        {
            get { return price; }
            set {
                price = value;
                NotifyPropertyChanged();
            }
        }

        public string PriceF
        {
            get { return string.Format("R {0:0.00}", price); }

        }

        public List<GeoData> RecommendedFor
        {
            get { return recommendedFor; }
            set {
                recommendedFor = value;
                NotifyPropertyChanged();
            }
        }

        public Guid GUID
        {
            get { return guid; }
            set {
                guid = value;
                NotifyPropertyChanged();
            }
        }

       
    }

    public partial class Option
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Contact objtoo = obj as Contact;
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

        public decimal calculateOverallCost()
        {
            decimal overallCost = 0;

            return overallCost;
        }

        public void saveOptionToDatabase()
        {
            Option option = this;
            DataHandler datahandler = new DataHandler(new DataHandlerHelper());

            datahandler.dynamicInsertQuery<Option>(option);

            foreach (string function in option.functions)
            {
                string query = string.Format(QueryStrings.Option.insertOptionFunction, Guid.NewGuid(), function, option.GUID);

                DataTable inserted = DatabaseHandler.getInstance().getFromStringQuery(query);
            }
           

            
            return;
        }

        public void UpdateOption()
        {
            DataHandler datahandler = new DataHandler(new DataHandlerHelper());

            string query = string.Format(QueryStrings.Option.deleteFunction, this.GUID);

            DataTable deleted = DatabaseHandler.getInstance().getFromStringQuery(query);

            foreach (string function in this.functions)
            {
                string subQuery = string.Format(QueryStrings.Option.insertOptionFunction, Guid.NewGuid(), function, this.GUID);

                DataTable inserted = DatabaseHandler.getInstance().getFromStringQuery(subQuery);
            }

            datahandler.dynamicUpdateQuery<Option>(this);
            return;
        }

        public void deleteOption()
        {
            Dictionary<string, KeyValuePair<string, string>> parameters = new Dictionary<string, KeyValuePair<string, string>>()
            {
                { QueryBuilder.OptionDelete.OptionGuid, new KeyValuePair<string,string>(this.GUID.ToString(), "GUID") },
            };

            bool status = DatabaseHandler.getInstance().nonQuery(QueryBuilder.OptionDelete.sp, parameters);
            return;

        }

        public ValidationResult validateInputs<T>(object target)
        {
            OptionValidation<T> validatation = new OptionValidation<T>();
            return validatation.validate<T>(target);   
        }

    }
}
