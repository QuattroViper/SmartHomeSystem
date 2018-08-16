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
    public class AppointmentLazy : Appointment, ILazyLoad<List<Appointment>>
    {

        private object locker = new object();
        private List<Appointment> appointmentLazy;


        public AppointmentLazy()
        {

        }

        public AppointmentLazy(Guid ClientGuid)
        {
            this.appointmentLazy = lazyLoad(ClientGuid);
        }

        AppointmentLazy(Guid guid, DateTime date, DateTime time, string operation, Guid clientGuid, decimal cost, string extraDetails, bool confirmed, bool completed) : base(guid, date, time, operation, clientGuid, cost, extraDetails, confirmed, completed)
        {

        }

        public List<Appointment> AppointmentLazyList
        {
            get { return appointmentLazy; }
            set { appointmentLazy = value; }
        }

        List<Appointment> DefaultAppointmentLazy
        {
            get { return new List<Appointment>(); }
        }

        public List<Appointment> lazyLoad(Guid guid)
        {
            List<Appointment> appointmentLazylist = new List<Appointment>();

            try
            {

                lock (locker)
                {
                    string query = string.Format(functions.QueryStrings.Client.getClientAppointmentsOnGuid, guid);

                    DataTable appointmentData = DatabaseHandler.getInstance().getFromStringQuery(query);

                    if (appointmentData.Rows.Count != 0)
                    {
                        foreach (DataRow row in appointmentData.Rows)
                        {
                            appointmentLazylist.Add(new Appointment(new Guid(row["guid"].ToString()), Convert.ToDateTime(row["date"].ToString()), Convert.ToDateTime(row["date"].ToString()).Date.Add(Convert.ToDateTime(row["time"].ToString()).TimeOfDay), (string)row["operation"], new Guid(row["client_guid"].ToString()), Convert.ToDecimal(row["cost"]), (string)row["extra_details"], Convert.ToBoolean(row["confirmed"]), Convert.ToBoolean(row["completed"])));
                        }
                    }
                    else
                    {
                        appointmentLazylist = DefaultAppointmentLazy;
                    }
                }


            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true);
                appointmentLazy = DefaultAppointmentLazy;
            }


            return appointmentLazylist;
        }



    }
}
