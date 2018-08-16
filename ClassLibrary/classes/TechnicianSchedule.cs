using ClassLibrary.functions;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public class TechnicianSchedule
    {
        private Guid guid;
        private Guid technicianGuid;
        private Guid appointmentGuid;

        public TechnicianSchedule()
        {

        }

        public TechnicianSchedule(Guid guid, Guid technicianGuid, Guid appointmentGuid)
        {
            this.guid = guid;
            this.technicianGuid = technicianGuid;
            this.appointmentGuid = appointmentGuid;
        }

        public Guid AppointmentGUID
        {
            get { return appointmentGuid; }
            set { appointmentGuid = value; }
        }


        public Guid TechnicianGUID
        {
            get { return technicianGuid; }
            set { technicianGuid = value; }
        }


        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public void insertTechnicianAppointment()
        {
            string query = string.Format(QueryStrings.Technician.insertTechnicianAndSchedule, this.GUID, this.TechnicianGUID, this.appointmentGuid);

            DataTable data = DatabaseHandler.getInstance().getFromStringQuery(query);

        }

        public bool checkIfTechnicianIsAvailable(DateTime time, Guid technicianGuid)
        {
            string query = string.Format(QueryStrings.Technician.isTechnicianAvaiable,time,time,technicianGuid);

            DataTable data = DatabaseHandler.getInstance().getFromStringQuery(query);

            int isAvailable = Convert.ToInt32(data.Rows[0]["occupied"]);

            if (isAvailable == 1)
            {
                return false;
            }
                else
            {
                return true;
            }
        }

    }
}
