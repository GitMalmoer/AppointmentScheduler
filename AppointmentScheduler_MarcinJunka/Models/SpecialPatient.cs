using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler_MarcinJunka.Models
{
    public class SpecialPatient : Patient
    {
        public SpecialPatient(string name,string email,string phone) : base(name,email,phone)
        {
            PatientIdentifier = GenerateIdentifier();
        }

        public override string GenerateIdentifier()
        {
            return "INV_" + GenerateHash();
        }
    }
}
