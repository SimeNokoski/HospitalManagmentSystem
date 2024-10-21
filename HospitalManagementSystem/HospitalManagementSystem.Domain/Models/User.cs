using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.Domain.Models
{
    public class User : BaseEntity
    {
       public string UserName { get; set; }
       public string Password { get; set; }
       public string Email { get; set; }
       public Role Role { get; set; }
       
       public Doctor Doctor { get; set; }
       public Patients Patients { get; set; }   

    }
}
