using System;
using ClassLibrary.interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public class Employee : Person
    {

        private Guid guid;
        private int permissionLevel;
        private bool active;
        private Login login;

        public Login Login
        {
            get { return login; }
            set { login = value; }
        }


        public bool Active
        {
            get { return active; }
            set { active = value; }
        }


        public int PermissionLevel
        {
            get { return permissionLevel; }
            set { permissionLevel = value; }
        }


        public Employee()
        {

        }

        public Employee(Guid guid, int permissionLevel, bool active, Login login)
        {
            this.guid = guid;
            this.permissionLevel = permissionLevel;
            this.active = active;
            this.login = login.decryptString();
        }

        public Employee(Guid guid, int permissionLevel, bool active, Login login, Id<Person> id, string name, string surname, DateTime dateOfBirth, string gender)
            : base(guid, id, name, surname, dateOfBirth, gender)
        {
            this.guid = guid;
            this.permissionLevel = permissionLevel;
            this.active = active;
            this.login = login.decryptString();
        }

        public new Guid GUID
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
