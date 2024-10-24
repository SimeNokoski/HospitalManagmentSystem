using HospitalManagementSystem.Domain.Models;
using HospitalManagementSystem.DTO.AppointmentsDtos;

namespace HospitaManagmentSystem.Mapper
{
    public static class AppointmentMapper
    {
        public static Appointments ToAppoinment(this CreateAppointmentDto createAppointmentDto)
        {
            return new Appointments
            {
                Status = false,
                DateTime = createAppointmentDto.DateTime,
                PatientId = null
            };
        }


        public static GetAppointmentsByDoctorId ToAppointmentDto(this Appointments appointments)
        {
            return new GetAppointmentsByDoctorId
            {
                Status = appointments.Status,
                DateTime = appointments.DateTime,
                DoctorFullName = $"{appointments.Doctor.FirstName} {appointments.Doctor.LastName}",
                DoctorId = appointments.DoctorId,
                PatientId = appointments.PatientId,
            };
        }

    }
}
