using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentScheduler_MarcinJunka.Models;

namespace AppointmentScheduler_MarcinJunka.Managers
{
    public class PatientManager
    {
        private List<Patient> _patients;

        public int Count
        {
            get
            {
                return _patients.Count;
            }
        }

        public PatientManager()
        {
            _patients = new List<Patient>();
        }

        public void addPatientToList(Patient patient)
        {
            if (patient != null)
            {
                _patients.Add(patient);
            }
        }

        public Patient getPatientById(int id)
        {
            if (id >= 0)
            {
                return _patients[id];
            }
            else
            {
                return null;
            }
        }

        public List<Patient> getPatients()
        {
            List<Patient> newPatients = new List<Patient>(_patients);
            return newPatients;
        }
    }
}
