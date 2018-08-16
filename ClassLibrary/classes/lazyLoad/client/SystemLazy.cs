using ClassLibrary.functions;
using DataAccess;
using ErrorHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes.lazyLoad.client
{
    public class SystemLazy : System, ILazyLoad<List<System>>
    {

        private object locker = new object();
        private List<System> systems;

        public SystemLazy()
        {

        }

        public SystemLazy(Guid guid) : base()
        {
            this.systems = lazyLoad(guid);
        }

        SystemLazy(Guid guid, List<Option> options, Status status, Contract contract) : base(guid, options, status, contract)
        {

        }

        public List<System> Systems
        {
            get { return systems; }
            set { systems = value; }
        }

        List<System> DefaultSystemList
        {
            get { return new List<System>(); }
        }

        public List<System> lazyLoad(Guid guid)
        {
            List<System> systemList = new List<System>();										
            try
            {
                 
                lock (locker)
                {
                    string query = string.Format(QueryStrings.getSystemBasedOnClientGuid, guid);

                    DataTable systemData = DatabaseHandler.getInstance().getFromStringQuery(query);

                    if (systemData.Rows.Count != 0)
                    {
                        foreach (DataRow row in systemData.Rows)
                        {
                            systemList.Add(new System(
                                new Guid(row["system_guid"].ToString()),
                                new List<Option>(),
                                new Status(new Guid(row["status_guid"].ToString()), Convert.ToInt32(row["critical"]), Convert.ToInt32(row["warning"]), (string)row["message"], null, null),
                                new Contract(new Guid(row["contract_guid"].ToString()), Convert.ToBoolean(row["is_active"]), (string)row["contract_identifier"], (string)row["service_level"], Convert.ToDateTime(row["start_date"]), Convert.ToDateTime(row["end_date"]), (string)row["upgrade_options"])
                                )
                            );
                        }
                    }
                    else
                    {

                    }
                }


            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
                systemList = DefaultSystemList;
            }
            return systemList;
        }

    }
}
