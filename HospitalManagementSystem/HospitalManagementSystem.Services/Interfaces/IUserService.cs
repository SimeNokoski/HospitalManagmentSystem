using HospitalManagementSystem.DTO.UserDtos;

namespace HospitalManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        void Register(RegisterUserDto registerUserDto);
        string LoginUser(LoginUserDto loginUserDto);
       
    }
}
