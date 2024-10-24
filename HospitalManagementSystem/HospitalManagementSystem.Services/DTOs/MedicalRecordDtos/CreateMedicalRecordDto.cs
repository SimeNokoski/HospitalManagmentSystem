namespace HospitalManagementSystem.DTO.MedicalRecordDtos
{
    public class CreateMedicalRecordDto
    {
        public string Treatment { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public string Diagnosis { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PatientId { get; set; }

    }
}
