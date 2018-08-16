using ClassLibrary.functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public partial class Login
    {
        private Guid guid;
        private string username;
        private string password;
        private int? loggedInTime;
        private bool? sessionActive;
        private string encryptedString;


        public Login()
        {
            
        }

        public Login(string encryptedString)
        {
            this.encryptedString = encryptedString;
        }

        public Login(Guid guid, string username, string password, int? loggedInTime, bool? sessionActive)
        {
            this.guid = guid;
            this.username = username;
            this.password = password;
            this.loggedInTime = loggedInTime;
            this.sessionActive = sessionActive;
        }

        public string EncryptedString
        {
            get { return this.encryptString(); }
            set { encryptedString = value; }
        }

        public bool? SessionActive
        {
            get { return sessionActive; }
            set { sessionActive = value; }
        }


        public int? LoggedInTime
        {
            get { return loggedInTime; }
            set { loggedInTime = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

    }

    public partial class Login
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        //private Guid guid;
        //private string username;
        //private string password;
        //private int? loggedInTime;
        //private bool? sessionActive;
        //private string encryptedString;
        public override string ToString()
        {
            return string.Format("{0}~{1}~{2}", this.GUID, this.Username, this.Password);
        }

        public void signOutOperator()
        {

        }

        public string encryptString()
        {
            //public string encryptString(string password) {
            //return Cryptography.encryptString(this.ToString(), password);
            return Cryptography.encryptString(this.ToString(), "bElgIUmcAmpUs123!?@201818-3a");
        }

    public Login decryptString()
    //public Login decryptString(string password)
    {
            //string decryptedString = Cryptography.decryptString(this.EncryptedString, password);
            string decryptedString = Cryptography.decryptString(this.EncryptedString, "bElgIUmcAmpUs123!?@201818-3a");

            return this;
        }
    }
}
