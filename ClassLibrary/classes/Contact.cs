using ClassLibrary.classes.validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using mailer = System.Net;

namespace ClassLibrary.classes
{
    public class Contact
    {
        private Guid guid;
        private string contactNumber;   // (xxx) xxx - xxxx
        private string emailAddress;
        private string androidDeviceID;
        private string appleDeviceID;
        private int[] contactMethods = { 0, 0, 0 };


        public Contact()
        {

        }

        public Contact(Guid guid, string contactNumber, string emailAddress, string androidDeviceID, string appleDeviceID, int[] contactMethods)
        {
            this.guid = guid;
            this.contactNumber = contactNumber;
            this.emailAddress = emailAddress;
            this.androidDeviceID = androidDeviceID;
            this.contactMethods = contactMethods;
            this.appleDeviceID = appleDeviceID;
        }

        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public string AppleDeviceID
        {
            get { return appleDeviceID; }
            set { appleDeviceID = value; }
        }

        public int[] ContactMethods
        {
            get { return contactMethods; }
            set { contactMethods = value; }
        }

        public string AndroidDeviceID
        {
            get { return androidDeviceID; }
            set { androidDeviceID = value; }
        }


        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        public string ContactNumber
        {
            get { return contactNumber; }
            set { contactNumber = value; }
        }

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

        public ValidationResult validateInputs<T>(object target)
        {
            OptionValidation<T> validatation = new OptionValidation<T>();
            return validatation.validate<T>(target);
        }

        public bool validateEmail(string email)
        {
            try
            {
                var addr = new mailer.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch 
            {
                return false;
            }
        }
    }
}
