using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.Domain.Models;
using HospitalManagementSystem.DTO.DoctorDtos;
using HospitalManagementSystem.Services.Interfaces;
using HospitaManagmentSystem.Mapper;
using System.Text;
using XSystem.Security.Cryptography;

namespace HospitalManagementSystem.Services.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IUserRepository _userRepository;

        public DoctorService(IDoctorRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public void CreateDoctor(DoctorDto doctorDto)
        {
           
            _repository.Add(doctorDto.ToDoctor());
        }

        public void DeleteDoctor(int id)
        {
            var doctor = _repository.GetById(id);
            _repository.Delete(doctor);
        }

        public void UpdateDoctor(UpdateDoctorDto updateDoctorDto, int userId)
        {
           var doctor = _repository.GetAll().FirstOrDefault(x=>x.UserId == userId);
           var user = _userRepository.GetById(userId);

            doctor.Age = updateDoctorDto.Age;
            doctor.FirstName = updateDoctorDto.FirstName;
            doctor.LastName = updateDoctorDto.LastName;
            doctor.Specialization = updateDoctorDto.Specialization;

            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(updateDoctorDto.Password));
            var hashedPassword = Encoding.ASCII.GetString(md5data);

            user.Password = hashedPassword;
            user.Email = updateDoctorDto.Email;
            user.UserName = updateDoctorDto.UserName;

            _userRepository.Update(user);
            _repository.Update(doctor);

        }
    }
}