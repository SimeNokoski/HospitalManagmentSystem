using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.DTO.DoctorDtos
{
    public class DoctorDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Specialization Specialization { get; set; }
    }
}
