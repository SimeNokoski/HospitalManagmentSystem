namespace HospitalManagementSystem.DTO.AppointmentsDtos
{
    public class GetAppointmentsByDoctorId
    {
        public DateTime DateTime { get; set; }  
        public bool Status { get; set; }       
        public int? PatientId { get; set; }    
        public int DoctorId { get; set; }     
        public string DoctorFullName { get; set; } 
    }
}
