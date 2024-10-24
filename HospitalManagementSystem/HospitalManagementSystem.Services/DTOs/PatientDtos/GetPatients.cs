namespace HospitalManagementSystem.DTO.PatientDtos
{
    public class GetPatients
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
    }
}