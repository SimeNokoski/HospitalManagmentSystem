using HospitalManagementSystem.Domain.Models;
using HospitalManagementSystem.DTO.AppointmentsDtos;
using HospitalManagementSystem.DTO.PatientDtos;

namespace HospitaManagmentSystem.Mapper
{
    public static class PatientMapper
    {
        public static Patients ToPatient(this PatientDto patientDto)
        {
            return new Patients
            {
                 Age = patientDto.Age,
                 FirstName = patientDto.FirstName,
                 LastName = patientDto.LastName,
                 User = new User
                 {
                     Email = patientDto.Email,
                     Password = patientDto.Password,
                     Role = HospitalManagementSystem.Domain.Enums.Role.Patient,
                     UserName = patientDto.UserName,
                 }
                       

            };
        }

        public static AllPatientByDtoctorIdDto ToAllPatientDto(this Patients patients)
        {
            return new AllPatientByDtoctorIdDto
            {
                FullName = $"{patients.FirstName} {patients.LastName}"
            };
        }

        public static GetPatients ToGetPatientDto(this Patients patients)
        {
            return new GetPatients
            {
                Id = patients.Id,
                FullName = $"{patients.FirstName} {patients.LastName}",
                Email = patients.User.Email,
                Age = patients.Age,
                UserName = patients.User.UserName
            };
        }
    }
}
