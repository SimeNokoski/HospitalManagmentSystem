namespace HospitalManagementSystem.Domain.Models
{
    public class Patients : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public List<Appointments> Appointments { get; set; }
        public List<MedicalRecord> MedicalRecords { get; set; }



    }
}
