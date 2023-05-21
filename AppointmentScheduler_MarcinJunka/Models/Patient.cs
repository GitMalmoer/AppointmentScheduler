using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler_MarcinJunka.Models
{
    public class Patient
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string PatientIdentifier { get; protected set; }

        public Patient(string name,string email,string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
            PatientIdentifier = GenerateIdentifier();
        }

        public virtual string GenerateIdentifier()
        {
            return GenerateHash();
        }

        public string GenerateHash()
        {
            Guid guid = Guid.NewGuid();
            var guidBytes = guid.ToByteArray();

            var base64Id = Convert.ToBase64String(guidBytes).Replace("-","");
            var shortId = base64Id.Substring(0, 5);

            return shortId;
        }

        public (string,string,string,string) GetPatientDetails()
        {
            return (Name, Email, Phone, PatientIdentifier);
        }

        public override string ToString()
        {
            return string.Format($"{Name} Id({PatientIdentifier})");
        }
    }
}
