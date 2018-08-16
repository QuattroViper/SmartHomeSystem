using ClassLibrary.classes.validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClassLibrary.classes
{
    public class Address
    {


        #region Fields
        private Guid guid;
        private string address1;
        private string address2;
        private string suburb;
        private string postalCode;
        private string city;
        private string country;

        #endregion



        #region Constructor

        public Address()
        {

        }

        public Address(Guid guid, string address1, string address2, string suburb, string postalCode, string city, string country)
        {
            this.guid = guid;
            this.address1 = address1;
            this.address2 = address2;
            this.suburb = suburb;
            this.postalCode = postalCode;
            this.city = city;
            this.country = country;
        }

        #endregion

        #region Properties

        public string TableName
        {
            get { return "tblAddress"; }
        }


        public SortedDictionary<string, string> TableDataInfo
        {
            get
            {
                return new SortedDictionary<string, string>() {
                    { "guid", "GUID"},
                    { "address1", "Address1" },
                    { "address2", "Address2" },
                    { "description", "Description" },
                    { "city", "City" },
                    { "country", "Country" },
                    { "postalCode", "PostalCode" },
                    { "suburb", "Suburb" }
                };
            }
        }

        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }


        public string Country
        {
            get { return country; }
            set { country = value; }
        }


        public string City
        {
            get { return city; }
            set { city = value; }
        }


        public string Suburb
        {
            get { return suburb; }
            set { suburb = value; }
        }


        public string Address2
        {
            get { return address2; }
            set { address2 = value; }
        }


        public string Address1
        {
            get { return address1; }
            set { address1 = value; }
        }

        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }
        #endregion


        #region Methods


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Address objtoo = obj as Address;
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

        public ValidationResult validateInputs<T>(object target)
        {
            OptionValidation<T> validatation = new OptionValidation<T>();
            return validatation.validate<T>(target);
        } 
        #endregion

    }
}
