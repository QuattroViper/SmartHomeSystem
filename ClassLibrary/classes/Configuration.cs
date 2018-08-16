using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public class Configuration
    {
        private string guid;
        List<Option> options;
        private string description;
        private string configurationCode;


        public Configuration()
        {

        }

        public Configuration(string guid, List<Option> options, string description, string configurationCode)
        {
            this.guid = guid;
            this.options = options;
            this.description = description;
            this.configurationCode = configurationCode;
        }

        public string ConfigurationCode
        {
            get { return configurationCode; }
            set { configurationCode = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public List<Option> Options
        {
            get { return options; }
            set { options = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Configuration objtoo = obj as Configuration;
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
