using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.DTO.UserDtos
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
