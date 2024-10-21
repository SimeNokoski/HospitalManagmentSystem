using HospitalManagementSystem.DTO.AppointmentsDtos;

namespace HospitalManagementSystem.Services.Interfaces
{
    public interface IAppointmentService
    {
        void AddAvailableAppointment(CreateAppointmentDto createAppointmentDto, int userId);
        void BookAppointment(BookAppointmentDto bookAppointmentDto, int userId);
        List<GetAppointmentsByDoctorId> GetAppointmentsByDoctorId(int userId);
        List<GetAppointmentsByDoctorId> AvailableAppointmentsByDoctorId(int userId);
        void RemoveAppointment(int id, int userId);
        void CancelAppointment(PatientCancelAppointmentDto patientCancelAppointmentDto, int patientId);
        List<AllPatientByDtoctorIdDto> AllPatientByDoctorId(int doctorId);
    }
}
