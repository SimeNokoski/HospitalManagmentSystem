namespace HospitalManagementSystem.DTO.DoctorDtos
{
    public class GetDoctorDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Specialization { get; set; }
    }
}