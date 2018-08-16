using ClassLibrary.classes.Combined;
using ClassLibrary.classes.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public class ServiceDelivery
    {


        #region Fields
        List<TechnicianDBInfo> techniciansList;
        Dictionary<ActiveDirectoryEmployee, AppointmentPriority> activeTechniciansList;
        List<AppointmentPriority> unassignedAppointments;
        #endregion


        #region Constructor
        public ServiceDelivery()
        {
            this.techniciansList = TechniciansList;
            this.activeTechniciansList = ActiveTechniciansList;
            this.unassignedAppointments = UnassignedAppointments;
        }
        #endregion

        #region Properties

        public List<TechnicianDBInfo> TechniciansList
        {
            get
            {
                TechnicianDBInfo ADTechnician = new TechnicianDBInfo();

                return ADTechnician.getAllTechniciansDBInfo();
            }

        }

        public Dictionary<ActiveDirectoryEmployee, AppointmentPriority> ActiveTechniciansList
        {
            get
            {
                return new AppointmentTechnician().getAllActiveTechniciansWithAppointment();
            }
        }

        public List<AppointmentPriority> UnassignedAppointments
        {
            get
            {
                return sortUnassignedAppointments();
            }
        }
        #endregion


        #region Methods


        /// <summary>
        /// Assing the Technician to a appontment.
        /// </summary>
        /// <param name="technicianGuid"></param>
        /// <param name="appointmentGuid"></param>
        public void assignTechnician(Guid technicianGuid, Guid appointmentGuid)
        {
            TechnicianSchedule technicianSchedule = new TechnicianSchedule(Guid.NewGuid(), technicianGuid, appointmentGuid);

            technicianSchedule.insertTechnicianAppointment();
        }

        /// <summary>
        /// Sort the unassigned appointments first on the priority and then the date. Uses a SortedSet vir IComparable and IComparer
        /// </summary>
        /// <returns></returns>
        public List<AppointmentPriority> sortUnassignedAppointments()
        {
            List<AppointmentPriority> usSortedAppointments = new AppointmentList().getAllUnassignedAppointments();

            SortedSet<AppointmentPriority> sorted = new SortedSet<AppointmentPriority>();

            // Check if there are immediate appointments that needs to be done!
            foreach (AppointmentPriority appointment in usSortedAppointments)
            {
                TimeSpan duration = appointment.Time - DateTime.UtcNow;

                if (duration.TotalDays <= 7)
                {
                    sorted.Add(appointment);
                }

            }

            if (sorted.Count != 0)
            {
                return sorted.ToList();
            }

            foreach (AppointmentPriority appointment in usSortedAppointments)
            {
                TimeSpan duration = appointment.Time - DateTime.UtcNow;

                sorted.Add(appointment);

            }



            //return sortedAppointments;
            return sorted.ToList();
        } 
        #endregion

    }
}
