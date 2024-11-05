namespace HospitalManagementSystem.DTO.MedicalRecordDtos
{
    public class MedicalRecordDto
    {
        public string Treatment { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public string Diagnosis { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PatientFullName { get; set; }
        public string DoctorFullName { get; set; }
    }
}