using ClassLibrary.functions;
using DataAccess;
using ErrorHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes.lazyLoad.client
{
    public class DetailsLazy : Client, ILazyLoad<DetailsLazy>
    {

        private object locker = new object();
        private DetailsLazy details;

        [databaseMap("gender")]
        private string genderC;

        public DetailsLazy(Guid clientGUID)
        {
            this.loadedDetails = lazyLoad(clientGUID);
        }

        public DetailsLazy()
        {

        }

        DetailsLazy(string genderC, Guid guid, List<Address> addresses, Login login, Contact contactDetails, string clientIdentifier, string id, string name, string surname, DateTime dateOfBirth, string gender) 
            : base(guid, addresses, login, contactDetails, clientIdentifier, id, name,  surname,  dateOfBirth,  gender)
        {
            this.genderC = genderC;
        }

        public DetailsLazy loadedDetails
        {
            get { return details; }
            set { details = value; }
        }

        public string GenderC
        {
            get { return genderC; }
            set { genderC = value; }
        }

        public DetailsLazy DefaultDetails
        {
            get { return new DetailsLazy(); }
        }

        public DetailsLazy lazyLoad(Guid guid)
        {
            DetailsLazy details;

            try
            {

                lock (locker)
                {
                    string clientAddressQuery = string.Format(QueryStrings.Client.getClientAddressesOnGuid, guid);

                    List<Address> addresses = new List<Address>();
                    DataTable resultsAddress = DatabaseHandler.getInstance().getFromStringQuery(clientAddressQuery);

                    if (resultsAddress.Rows.Count != 0)
                    {
                        foreach (DataRow row in resultsAddress.Rows)
                        {
                            addresses.Add(new Address(new Guid(row["guid"].ToString()), (string)row["address1"], (string)row["address2"], (string)row["suburb"], (string)row["postalCode"], (string)row["city"], (string)row["country"]));
                        }

                    }

                    string ClientDetailsQuery = string.Format(QueryStrings.Client.getClientDetailsOnGuid, guid);

                    DataTable results = DatabaseHandler.getInstance().getFromStringQuery(ClientDetailsQuery);

                    if (results.Rows.Count == 1)
                    {
                        Dictionary<string, object> idDecoded = new Dictionary<string, object>();
                        idDecoded = functions.functions.decodeID((string)results.Rows[0]["identification"]);

                        string gend = (string)idDecoded["gender"];
                        DateTime dated = (DateTime)idDecoded["date"];
                        Guid clientGuid = new Guid(results.Rows[0]["guid"].ToString());

                        Login login = new Login((string)results.Rows[0]["login_string"]).decryptString();
                        Contact contact = new Contact(new Guid(results.Rows[0]["contact_guid"].ToString()), (string)results.Rows[0]["contact_number"], (string)results.Rows[0]["email_address"], (string)results.Rows[0]["android_device_id"], (string)results.Rows[0]["apple_device_id"], results.Rows[0]["contact_methods"].ToString().Split(',').Select(n => int.Parse(n)).ToArray());


                        details = new DetailsLazy(gend, clientGuid, addresses, login, contact, (string)results.Rows[0]["client_identifier"], (string)results.Rows[0]["identification"], (string)results.Rows[0]["name"], (string)results.Rows[0]["surname"], dated, gend);
                    }
                    else
                    {
                        details = DefaultDetails;
                    }
                }
                
            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
                details = DefaultDetails;
            }

            return details;
        }

        //public static TAttribute GetAttribute<TAttribute>(this object value)
        //where TAttribute : Attribute
        //{
        //    var type = value.GetType();
        //    var name = Enum.GetName(type, value);
        //    return type.GetField(name) // I prefer to get attributes this way
        //        .GetCustomAttribute<TAttribute>();
        //}


    }
}
