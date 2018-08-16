using ClassLibrary.classes.validation;
using ClassLibrary.functions;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClassLibrary.classes
{
    public partial class Appointment
    {


        #region Fields
        private Guid guid;
        private DateTime date;
        private DateTime time;
        private string operation;
        private Guid clientGuid;
        private decimal cost;
        private string extraDetails;
        private bool confirmed;
        private bool completed;
        #endregion

        #region Constructors

        public Appointment()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <param name="operation"></param>
        /// <param name="clientGuid"></param>
        /// <param name="cost"></param>
        /// <param name="extraDetails"></param>
        /// <param name="confirmed"></param>
        /// <param name="completed"></param>
        public Appointment(Guid guid, DateTime date, DateTime time, string operation, Guid clientGuid, decimal cost, string extraDetails, bool confirmed, bool completed)
        {
            this.guid = guid;
            this.date = date;
            this.time = time;
            this.operation = operation;
            this.clientGuid = clientGuid;
            this.cost = cost;
            this.extraDetails = extraDetails;
            this.confirmed = confirmed;
            this.completed = completed;


        }
        #endregion

        #region Properties


        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        public bool Completed
        {
            get { return completed; }
            set { completed = value; }
        }

        public bool Confirmed
        {
            get { return confirmed; }
            set { confirmed = value; }
        }


        public string ExtraDetails
        {
            get { return extraDetails; }
            set { extraDetails = value; }
        }


        public decimal Cost
        {
            get { return cost; }
            set { cost = value; }
        }



        public Guid ClientGUID
        {
            get { return clientGuid; }
            set { clientGuid = value; }
        }


        public string Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        #endregion


    }

    public partial class Appointment
    {


        #region Methods

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Appointment objtoo = obj as Appointment;
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

        public void updateConfirmation()
        {
            int priority = 0;
            switch (this.operation)
            {
                case "Routine":
                priority = 5;
                break;
                case "Inspection":
                priority = 5;
                break;
                case "Update":
                priority = 4;
                break;
                case "Upgrade":
                priority = 3;
                break;
                case "Installation":
                priority = 2;
                break;
                case "Removal":
                priority = 2;
                break;
                default:
                priority = 6;
                break;
            }

            string subQuery = string.Format(QueryStrings.Schedule.insertIntoSchedule, Guid.NewGuid(), this.GUID, priority);

            // Do insert of appointment here.
            DataTable subData = DatabaseHandler.getInstance().getFromStringQuery(subQuery);

            string query = string.Format(QueryStrings.Appointment.updateAppointmentConfirmed, this.GUID);

            // Do insert of appointment here.
            DataTable data = DatabaseHandler.getInstance().getFromStringQuery(query);
        }

        public void updateCompleted()
        {
            string query = string.Format(QueryStrings.Appointment.updateAppointmentCompleted, this.GUID);

            // Do insert of appointment here.
            DataTable data = DatabaseHandler.getInstance().getFromStringQuery(query);


        }

        public void insertNewAppointment()
        {
            Guid appointmentGuid = Guid.NewGuid();

            string query = string.Format(QueryStrings.Appointment.insertAppointment, appointmentGuid, this.date, this.time, this.operation, clientGuid, this.cost, this.extraDetails, this.confirmed ? 1 : 0, this.completed ? 1 : 0);

            // Do insert of appointment here.
            DataTable data = DatabaseHandler.getInstance().getFromStringQuery(query);

            if (this.confirmed)
            {
                //insert to schedule

                int priority = 0;
                switch (this.operation)
                {
                    case "Routine":
                    priority = 5;
                    break;
                    case "Inspection":
                    priority = 5;
                    break;
                    case "Update":
                    priority = 4;
                    break;
                    case "Upgrade":
                    priority = 3;
                    break;
                    case "Installation":
                    priority = 2;
                    break;
                    case "Removal":
                    priority = 2;
                    break;
                    default:
                    priority = 6;
                    break;
                }

                string subQuery = string.Format(QueryStrings.Schedule.insertIntoSchedule, Guid.NewGuid(), appointmentGuid, priority);

                // Do insert of appointment here.
                DataTable subData = DatabaseHandler.getInstance().getFromStringQuery(subQuery);

            }
        }

        public ValidationResult validateInputs<T>(object target)
        {
            OptionValidation<T> validatation = new OptionValidation<T>();
            return validatation.validate<T>(target);
        }

        public void deleteAppointment()
        {

        } 
        #endregion

    }
}
