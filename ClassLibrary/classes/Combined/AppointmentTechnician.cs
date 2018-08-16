using ClassLibrary.classes.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes.Combined
{
    public class AppointmentTechnician
    {

        public AppointmentTechnician()
        {

        }

        public Dictionary<ActiveDirectoryEmployee,AppointmentPriority> getAllActiveTechniciansWithAppointment()
        {
            Dictionary<ActiveDirectoryEmployee, AppointmentPriority> activeTechnicians = new Dictionary<ActiveDirectoryEmployee, AppointmentPriority>();

            ActiveDirectoryEmployee ADTechnician = new ActiveDirectoryEmployee();

            AppointmentList appointmentPriority = new AppointmentList();
            List<AppointmentPriority> appointmentPriorityList = appointmentPriority.getAllActiveAppointments();

            foreach (AppointmentPriority appointment in appointmentPriorityList)
            {
                ADTechnician = ADTechnician.getSpecificTechnician(appointment.TechnicianGuid);
                activeTechnicians.Add(ADTechnician, appointment);
            }

            return activeTechnicians;
        }



    }
}
