using ClassLibrary.classes.Combined;
using ClassLibrary.functions;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes.Lists
{
    public class AppointmentList : AppointmentPriority
    {

        List<AppointmentPriority> appointments;

        public AppointmentList()
        {
            
        }


        public List<AppointmentPriority> Appointments
        {
            get { return appointments; }
            set { appointments = value; }
        }

        public List<AppointmentPriority> getAllUnassignedAppointments()
        {
            //List<AppointmentPriority> appointments = new List<AppointmentPriority>();
            appointments = new List<AppointmentPriority>();

            string query = string.Format(QueryStrings.AppointmentSchedule.getAllUnassignmentAppointments);

            DataTable data = DatabaseHandler.getInstance().getFromStringQuery(query);

            foreach (DataRow row in data.Rows)
            {
                appointments.Add( new AppointmentPriority(new Guid(row["guid"].ToString()), Convert.ToDateTime(row["date"].ToString()), Convert.ToDateTime(row["date"].ToString()).Date.Add(Convert.ToDateTime(row["time"].ToString()).TimeOfDay), (string)row["operation"], new Guid(row["client_guid"].ToString()), Convert.ToDecimal(row["cost"]), (string)row["extra_details"], Convert.ToBoolean(row["confirmed"]), Convert.ToBoolean(row["completed"]), Convert.ToInt32(row["priority"]))
                    );
            }

            return appointments;

        }



        public List<AppointmentPriority> getAllActiveAppointments()
        {
            //List<AppointmentPriority> appointments = new List<AppointmentPriority>();
            appointments = new List<AppointmentPriority>();

            string query = string.Format(QueryStrings.AppointmentSchedule.getAllActiveTechniciansAndAppointments);

            DataTable data = DatabaseHandler.getInstance().getFromStringQuery(query);

            foreach (DataRow row in data.Rows)
            {
                appointments.Add(
                        new AppointmentPriority(new Guid(row["guid"].ToString()), Convert.ToDateTime(row["date"].ToString()), Convert.ToDateTime(row["date"].ToString()).Date.Add(Convert.ToDateTime(row["time"].ToString()).TimeOfDay), (string)row["operation"], new Guid(row["client_guid"].ToString()), Convert.ToDecimal(row["cost"]), (string)row["extra_details"], Convert.ToBoolean(row["confirmed"]), Convert.ToBoolean(row["completed"]), Convert.ToInt32(row["priority"]), new Guid(row["technician_guid"].ToString()))
                    );
            }

            return appointments;
        }

    }
}
