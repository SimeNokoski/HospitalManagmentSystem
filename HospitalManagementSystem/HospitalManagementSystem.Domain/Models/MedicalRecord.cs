namespace HospitalManagementSystem.Domain.Models
{
    public class MedicalRecord : BaseEntity
    {
        public string Treatment { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public string Diagnosis { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Patients Patients { get; set; }
        public int PatientId { get; set; }

        public Doctor Doctor { get; set; }
        public int DoctorId { get; set; }
    }
}
