using ClassLibrary.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public partial class Schedule
    {
        private Dictionary<TimeSpan,Appointment> scheduleTimeActivity;
        private string guid;

        public Schedule()
        {

        }

        public Schedule(Dictionary<TimeSpan, Appointment> scheduleTimeActivity, string guid)
        {
            this.scheduleTimeActivity = scheduleTimeActivity;
            this.guid = guid;
        }

        public string GUID
        {
            get { return guid; }
            set { guid = value; }
        }


        public Dictionary<TimeSpan, Appointment> ScheduleTimeActivity
        {
            get { return scheduleTimeActivity; }
            set { scheduleTimeActivity = value; }
        }


    }

    public partial class Schedule
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Schedule objtoo = obj as Schedule;
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
