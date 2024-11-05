using HospitalManagementSystem.DTO.PatientDtos;

namespace HospitalManagementSystem.Services.Services.Interfaces
{
    public interface IPatientService
    {
        List<GetPatients> GetAllPatients(int userId);
        GetPatients GetPatientById(int userId, int id);
        void UpdatePatient(int userId, PatientDto patientDto);
        void DeletePatient(int id);
    }
}
