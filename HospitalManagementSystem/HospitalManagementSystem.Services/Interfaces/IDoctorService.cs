using HospitalManagementSystem.DTO.DoctorDtos;

namespace HospitalManagementSystem.Services.Interfaces
{
    public interface IDoctorService
    {
        void CreateDoctor(DoctorDto doctorDto, int userId);
        void DeleteDoctor(int doctorId, int userId);
        void UpdateDoctor(DoctorDto doctorDto, int userId);
        List<GetDoctorDto> GetAllDoctor(int userId);
        GetDoctorDto GetDoctorById(int userId, int doctorId);
        List<GetDoctorDto> GetAllDoctorsBySpecialization(int userId, string specialization);


    }
}
