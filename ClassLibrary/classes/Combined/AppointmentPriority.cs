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
    public class AppointmentPriority : Appointment, IComparer<AppointmentPriority>, IComparable<AppointmentPriority>
    {

        private int priority;
        private Guid technicianGuid;

        public AppointmentPriority()
        {

        }

        public AppointmentPriority(Guid guid, DateTime date, DateTime time, string operation, Guid clientGuid, decimal cost, string extraDetails, bool confirmed, bool completed, int priority)
            : base(guid, date, time, operation, clientGuid, cost, extraDetails, confirmed, completed)
        {
            this.priority = priority;
        }

        public AppointmentPriority(Guid guid, DateTime date, DateTime time, string operation, Guid clientGuid, decimal cost, string extraDetails, bool confirmed, bool completed, int priority, Guid technicianGuid) 
            : base(guid,date,time,operation,clientGuid,cost,extraDetails,confirmed,completed)
        {
            this.priority = priority;
            this.technicianGuid = technicianGuid;
        }

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }
        
        public Guid TechnicianGuid
        {
            get { return technicianGuid; }
            set { technicianGuid = value; }
        }

        public int Compare(AppointmentPriority x, AppointmentPriority y)
        {
            if (x.Priority != y.Priority) return x.Priority.CompareTo(y.Priority);
            return x.Time.CompareTo(y.Time); 
        }

        public int CompareTo(AppointmentPriority other)
        {
            if (this.Priority != other.Priority) return this.Priority.CompareTo(other.Priority);
            return this.Time.CompareTo(other.Time);
        }
    }
}
