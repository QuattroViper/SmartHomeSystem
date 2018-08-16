using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public class ContractIdentifier
    {


        #region Fields
        private string year;
        private string contractType;
        private string serviceLevel;
        private string randomNumbers;
        #endregion



        #region Constructors

        /// <summary>
        /// Constructor to create the Contract Identifier.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="serviceLevel"></param>
        /// <param name="year"></param>
        /// <param name="randomNumbers"></param>
        public ContractIdentifier(string contractType, string serviceLevel, string year = "", string randomNumbers = "")
        {
            this.year = year.Length == 0 ? DateTime.UtcNow.Year.ToString() : year;
            this.contractType = contractType;
            this.serviceLevel = serviceLevel;
            this.randomNumbers = randomNumbers.Length == 0 ? generateRandomNumbers() : randomNumbers;
        }
        #endregion


        #region Properties
        public string RandomNumbers
        {
            get { return randomNumbers; }
            set { randomNumbers = value; }
        }


        public string ServiceLevel
        {
            get { return serviceLevel; }
            set { serviceLevel = value; }
        }


        public string ContractType
        {
            get { return contractType; }
            set { contractType = value; }
        }


        public string Year
        {
            get { return year; }
            set { year = value; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Generate the Contract unique indentifier. 
        /// </summary>
        /// <returns>string contract identifier</returns>
        string generateRandomNumbers()
        {
            StringBuilder builder = new StringBuilder();

            Random rand = new Random();
            int number = rand.Next(0, 9999);

            for (int i = 0; i < 6 - number.ToString().Length; i++)
            {
                builder.Append("0");
            }

            builder.Append(number.ToString());

            return builder.ToString();
        }



        public override string ToString()
        {
            return string.Format("{0}{1}{2}{3}", this.year, this.contractType, this.serviceLevel, this.randomNumbers);
        } 
        #endregion

    }
}
