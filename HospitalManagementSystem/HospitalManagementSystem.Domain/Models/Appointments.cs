namespace HospitalManagementSystem.Domain.Models
{
    public class Appointments : BaseEntity
    {
        public DateTime DateTime { get; set; }
        public bool Status { get; set; }

        public Patients? Patient { get; set; }
        public int? PatientId { get; set; }

        public Doctor Doctor { get; set; }
        public int DoctorId { get; set;}
    }
}
