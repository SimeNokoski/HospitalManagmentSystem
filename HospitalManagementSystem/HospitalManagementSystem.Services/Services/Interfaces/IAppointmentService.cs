using HospitalManagementSystem.DTO.AppointmentsDtos;

namespace HospitalManagementSystem.Services.Services.Interfaces
{
    public interface IAppointmentService
    {
        void AddAvailableAppointment(CreateAppointmentDto createAppointmentDto, int userId);
        void BookAppointment(BookCancelAppointmentDto bookAppointmentDto, int userId);
        List<GetAppointmentsByDoctorId> GetAppointmentsByDoctorId(int userId);
        List<GetAppointmentsByDoctorId> AvailableAppointmentsByDoctorId(int doctorId);
        void RemoveAppointment(int id, int userId);
        void CancelAppointment(BookCancelAppointmentDto bookCancelAppointmentDto, int patientId);
        List<AllPatientByDtoctorIdDto> AllPatientByDoctorId(int doctorId);
    }
}
