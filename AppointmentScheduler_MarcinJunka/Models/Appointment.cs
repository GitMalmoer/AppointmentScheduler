using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentScheduler_MarcinJunka.Data;

namespace AppointmentScheduler_MarcinJunka.Models
{
    public record Appointment
    {
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public Patient Patient { get; set; }
        public ServicesEnum Service { get; set; }

        public override string ToString()
        {
            return string.Format($"Id: {Patient.PatientIdentifier} \nDate: {Date.ToShortDateString()} \nTime: {Time} \nService: {Service}");
        }
    }
}
