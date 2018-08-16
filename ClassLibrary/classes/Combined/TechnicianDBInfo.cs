using ClassLibrary.functions;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes.Combined
{
    public class TechnicianDBInfo : ActiveDirectoryEmployee
    {

        private int totalAppointments;
        private ActiveDirectoryEmployee adTechnician;

        public ActiveDirectoryEmployee ADTechnician
        {
            get { return adTechnician; }
            set { adTechnician = value; }
        }


        public TechnicianDBInfo()
        {

        }

        public TechnicianDBInfo(int totalAppointments, ActiveDirectoryEmployee adTechnician)
        {
            this.totalAppointments = totalAppointments;
            this.adTechnician = adTechnician;
        }

        public int TotalAppointments
        {
            get { return totalAppointments; }
            set { totalAppointments = value; }
        }

        public List<TechnicianDBInfo> getAllTechniciansDBInfo()
        {
            List<ActiveDirectoryEmployee> tecniciansFromAD = getAllTechnicians();
            List<TechnicianDBInfo> techniciansDB = new List<TechnicianDBInfo>();

            foreach (ActiveDirectoryEmployee technician in tecniciansFromAD)
            {

                string query = string.Format(QueryStrings.Technician.getTotalAppointmentsDoneByTechnician,technician.GUID);

                DataTable data = DatabaseHandler.getInstance().getFromStringQuery(query);

                techniciansDB.Add(
                        new TechnicianDBInfo(Convert.ToInt32(data.Rows[0]["total_appointments"]),technician)
                    );
            }


            return techniciansDB;
        }


    }
}
