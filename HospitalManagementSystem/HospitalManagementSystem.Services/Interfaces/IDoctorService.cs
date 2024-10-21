using HospitalManagementSystem.DTO.DoctorDtos;

namespace HospitalManagementSystem.Services.Interfaces
{
    public interface IDoctorService
    {
        void CreateDoctor(DoctorDto doctorDto);
        void DeleteDoctor(int id);
        void UpdateDoctor(UpdateDoctorDto updateDoctorDto, int userId);
        

  
    }
}
