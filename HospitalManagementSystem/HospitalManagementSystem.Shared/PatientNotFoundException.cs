using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Shared
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException(string message) : base(message) { }
    }
}
