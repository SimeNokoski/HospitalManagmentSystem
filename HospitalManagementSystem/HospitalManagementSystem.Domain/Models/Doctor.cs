using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.Domain.Models
{
    public class Doctor : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Specialization? Specialization { get; set; }

        public User User { get; set; }  
        public int UserId { get; set; }

        public List<Appointments> Appointments { get; set; }
        public List<MedicalRecord> MedicalRecords { get; set; }

    }
}
