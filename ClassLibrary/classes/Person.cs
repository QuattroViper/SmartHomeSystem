using ClassLibrary.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public class Person
    {
        private Guid guid;
        private string id;
        private string name;
        private string surname;
        private DateTime dateOfBirth;
        private string gender;            //0 = Female, 1 = Male



        private static List<Person> personList;


        public Person()
        {
            
        }

        public Person(Guid guid, string name, string surname)
        {
            this.guid = guid;
            this.name = name;
            this.surname = surname;
        }

        public Person(Guid guid, string id, string name, string surname)
        {
            this.guid = guid;
            this.id = id;
            this.name = name;
            this.surname = surname;
        }

        public Person(Guid guid, string id, string name, string surname, DateTime dateOfBirth, string gender)
        {
            this.guid = guid;
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.dateOfBirth = dateOfBirth;
            this.gender = gender;
        }


        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }


        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        
        public static List<Person> PersonList
        {
            get { return personList;  }
            set { personList = value; }
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set {
                // Convert correct from database or from input
                dateOfBirth = value;
            }
        }

        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Person objtoo = obj as Person;
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


    }
}
