using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentScheduler_MarcinJunka.Models;

namespace AppointmentScheduler_MarcinJunka.Managers
{
    public class AppointmentManager
    {
        private List<Appointment> Appointments;

        public AppointmentManager()
        {
            Appointments = new List<Appointment>();
        }
        public void AddToList(Appointment appointment)
        {
            if (appointment == null)
            {
                return;
            }

            Appointments.Add(appointment);
        }

        public List<Appointment> GetPatientAppointments(Patient patient)
        {
            if (patient == null)
            {
                return null;
            }

            List<Appointment> appointments = Appointments.Where(p => p.Patient.PatientIdentifier == patient.PatientIdentifier).ToList();

            if (appointments != null && appointments.Any())
            {
                return appointments;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Any modifications made to the returned list will not affect the original Appointments list maintained by the AppointmentManager class
        /// </summary>
        /// <returns></returns>
        public List<Appointment> GetAllAppointments()
        {
            // as because main Appointments field is encapsulated we give out only copy of appointments
            List<Appointment> newListOfAppointments = new List<Appointment>(Appointments);
            return newListOfAppointments;
        }
    }
}
