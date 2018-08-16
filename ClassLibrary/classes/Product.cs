using ClassLibrary.functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public partial class Product: INotifyPropertyChanged
    {
        //[colname('guid')]
        private Guid guid;
        private string productCode;
        private string name;
        private string description;
        private decimal price;
        private string type;            // Sensor, Service, Component, etc
        private int warrentyPeriod;
        private bool inStock;
        private DateTime arrivalDate;
        private string manufacturer;
        private string model;
        private string serialNumber;

        public event PropertyChangedEventHandler PropertyChanged;

        public Product()
        {

        }

        public Product(Guid guid, string productCode, string name, string description, decimal price, string type, int warrentyPeriod, bool inStock, DateTime arrivalDate, string manufacturer, string model, string serialNumber)
        {
            this.guid = guid;
            this.productCode = productCode;
            this.name = name;
            this.description = description;
            this.price = price;
            this.type = type;
            this.warrentyPeriod = warrentyPeriod;
            this.inStock = inStock;
            this.arrivalDate = arrivalDate;
            this.manufacturer = manufacturer;
            this.model = model;
            this.serialNumber = serialNumber;
        }

        public string TableName
        {
            get { return "dbo.tblProduct";  }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public SortedDictionary<string, string> TableDataInfo
        {
            get
            {
                return new SortedDictionary<string, string>() {
                    { "guid", "GUID"},
                    { "product_code", "ProductCode" },
                    { "name", "Name" },
                    { "description", "Description" },
                    { "price", "Price" },
                    { "type", "Type" },
                    { "warrenty_period", "WarrentyPeriod" },
                    { "in_stock", "InStock" },
                    { "arrival_date", "ArrivalDate" },
                    { "manufacturer", "Manufacturer" },
                    { "model", "Model" },
                    { "serial_number", "SerialNumber" },
                };
            }
        }

        public string SerialNumber
        {
            get { return serialNumber; }
            set {
                serialNumber = value;
                NotifyPropertyChanged();
            }
        }


        public string Model
        {
            get { return model; }
            set {
                model = value;
                NotifyPropertyChanged();
            }
        }



        public string Manufacturer
        {
            get { return manufacturer; }
            set {
                manufacturer = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime ArrivalDate
        {
            get { return arrivalDate; }
            set {
                arrivalDate = value;
                NotifyPropertyChanged();
            }
        }


        public bool InStock
        {
            get { return inStock; }
            set {
                inStock = value;
                NotifyPropertyChanged();
            }
        }


        public string ProductCode
        {
            get { return productCode; }
            set {
                productCode = value;
                NotifyPropertyChanged();
            }
        }

        public string Name {
            get { return name; }
            set {
                name = value;
                CatchInserts.notifyOfNewInsert("Product");
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

        public decimal Price
        {
            get { return price; }
            set {
                price = value;
                NotifyPropertyChanged();
            }
        }

        public string Type
        {
            get { return type; }
            set {
                type = value;
                NotifyPropertyChanged();
            }
        }

        public int WarrentyPeriod
        {
            get { return warrentyPeriod; }
            set {
                warrentyPeriod = value;
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

    public partial class Product
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Product p = obj as Product;
            if ((object)p == null)
            {
                return false;
            }

            return (this.GUID == p.GUID);
        }

        public override int GetHashCode()
        {
            return this.GUID.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public void AddProduct()
        {

        }

        public void RemoveProduct()
        {

        }

        public void UpdateProduct()
        {

        }

    }
}
