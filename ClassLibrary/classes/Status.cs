using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public partial class Status
    {
        private Guid guid;
        private int citical;            // Failures
        private int warnings;           // firmware of out of date software
        private string messages;
        private decimal? averageTemperature;
        private decimal? averageEnegeryUsage;

        public Status()
        {

        }

        public Status(Guid guid, int citical, int warnings, string messages, decimal? averageTemperature, decimal? averageEnegeryUsage) {
            this.guid = guid;
            this.citical = citical;
            this.warnings = warnings;
            this.messages = messages;
            this.averageTemperature = averageTemperature;
            this.averageEnegeryUsage = averageEnegeryUsage;
        }

        public decimal? AverageEnergyUsage
        {
            get { return averageEnegeryUsage; }
            set { averageEnegeryUsage = value; }
        }


        public decimal? AverageTemperature
        {
            get { return averageTemperature; }
            set { averageTemperature = value; }
        }

        public int Warning
        {
            get { return warnings; }
            set { warnings = value; }
        }

        public string Messages
        {
            get { return messages; }
            set { messages = value; }
        }


        public int Citical
        {
            get { return citical; }
            set { citical = value; }
        }


        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

       
    }

    public partial class Status
    {
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

        public void notifyParties()
        {

        }
    }
}
