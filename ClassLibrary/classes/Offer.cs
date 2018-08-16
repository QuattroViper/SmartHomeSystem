using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public class Offer
    {
        private string guid;
        private Option option;
        private decimal discount;
        private DateTime startDate;
        private DateTime endDate;
        private string description;

        public Offer()
        {

        }

        public Offer(string guid, Option option, decimal discount, DateTime startDate, DateTime endDate, string description)
        {
            this.guid = guid;
            this.option = option;
            this.discount = discount;
            this.startDate = startDate;
            this.endDate = endDate;
            this.description = description;
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        public DateTime MyProperty
        {
            get { return endDate; }
            set { endDate = value; }
        }


        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }



        public decimal Discount
        {
            get { return discount; }
            set { discount = value; }
        }



        public string GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
