using ClassLibrary.classes.validation;
using ClassLibrary.functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClassLibrary.classes
{
    public partial class Card
    {
        private Guid guid;
        private string bank;
        private string paymentType;
        private string cardNumber;
        private DateTime expireDate;
        private string cardHolder;
        private string cvvNumber;
        private string encryptedString;

        public Card()
        {

        }

        public Card(string encryptedString)
        {
            this.encryptedString = encryptedString;
        }

        public Card(Guid guid, string bank, string paymentType, string cardNumber, DateTime expireDate, string cardHolder, string cvvNumber)
        {
            this.guid = guid;
            this.paymentType = paymentType;
            this.cardNumber = cardNumber;
            this.expireDate = expireDate;
            this.cardHolder = cardHolder;
            this.cvvNumber = cvvNumber;
            this.bank = bank;
        }

        public string Bank
        {
            get { return bank; }
            set { bank = value; }
        }

        public string EncryptedString
        {
            get { return this.encryptObject(); }
            set { encryptedString = value; }
        }


        public string CVVNumber
        {
            get { return cvvNumber; }
            set { cvvNumber = value; }
        }


        public string CardHolder
        {
            get { return cardHolder; }
            set { cardHolder = value; }
        }


        public DateTime ExpireDate
        {
            get { return expireDate; }
            set { expireDate = value; }
        }



        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }


        public string PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; }
        }


        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

    }

    public partial class Card
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Card objtoo = obj as Card;
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
            return string.Format("{0}~{1}~{2}~{3}~{4}~{5}~{6}", this.GUID,this.Bank, this.PaymentType, this.CardNumber, this.ExpireDate, this.CardHolder, this.CVVNumber); 
        }

        public string encryptObject()
        {

            return Cryptography.encryptString(this.ToString(), "bElgIUmcAmpUs123!?@201818-3a");
        }

        public Card decryptString()
        {

            string decryptedString = Cryptography.decryptString(this.EncryptedString,"bElgIUmcAmpUs123!?@201818-3a");

            string[] splitted = decryptedString.Split('~');

            this.guid = new Guid(splitted[0]);
            this.bank = splitted[1];
            this.paymentType = splitted[2];
            this.cardNumber = splitted[3];
            this.expireDate = DateTime.Parse(splitted[4]);
            this.cardHolder = splitted[5];
            this.cvvNumber = splitted[6];

            //this.guid = somestring

            return this;
        }

        public ValidationResult validateInputs<T>(object target)
        {
            OptionValidation<T> validatation = new OptionValidation<T>();
            return validatation.validate<T>(target);
        }
    }
}
