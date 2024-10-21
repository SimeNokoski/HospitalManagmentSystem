using HospitalManagementSystem.Domain.Models;
using HospitalManagementSystem.DTO.DoctorDtos;
using System.Text;
using XSystem.Security.Cryptography;

namespace HospitaManagmentSystem.Mapper
{
    public static class DoctorMapper
    {
        public static Doctor ToDoctor(this DoctorDto doctorDto)
        {
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(doctorDto.Password));
            var hashedPassword = Encoding.ASCII.GetString(md5data);

            return new Doctor
            {
                Specialization = doctorDto.Specialization,
                Age = doctorDto.Age,
                FirstName = doctorDto.FirstName,
                LastName = doctorDto.LastName,
                User = new User
                {
                    Email = doctorDto.Email,
                    Password = hashedPassword,
                    UserName = doctorDto.UserName,
                    Role = HospitalManagementSystem.Domain.Enums.Role.Doctor
                }
            };
        }

     
    }
}


