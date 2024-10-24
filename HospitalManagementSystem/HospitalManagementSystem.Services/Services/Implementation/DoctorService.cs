using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.DTO.DoctorDtos;
using HospitalManagementSystem.Services.Services.Interfaces;
using HospitalManagementSystem.Shared;
using HospitaManagmentSystem.Mapper;
using System.Text;
using System.Text.RegularExpressions;
using XSystem.Security.Cryptography;

namespace HospitalManagementSystem.Services.Services.Implementation
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

        public void CreateDoctor(DoctorDto doctorDto, int userId)
        {
            var superAdmin = _userRepository.GetById(userId);
            if (superAdmin == null || superAdmin.Role != Domain.Enums.Role.SuperAdmin)
            {
                throw new UnauthorizedAccessException("You cannot to add a doctor");
            }

            ValidateDoctor(doctorDto);

            var doctor = doctorDto.ToDoctor();
            _repository.Add(doctor);
        }

        public void DeleteDoctor(int doctorId)
        {
            var doctor = _repository.GetById(doctorId);
            if (doctor == null)
            {
                throw new DoctorNotFoundException($"Doctor with id {doctorId} not found");
            }
            var user = _userRepository.GetAll().FirstOrDefault(x => x.Id == doctor.UserId);
            if (user == null)
            {
                throw new Exception("user not found");
            }
            _repository.Delete(doctor);
            _userRepository.Delete(user);
        }

        public List<GetDoctorDto> GetAllDoctor(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new Exception($"user with id {userId} not found");
            }
            var doctors = _repository.GetAll();
            if (!doctors.Any())
            {
                throw new DoctorNotFoundException("no doctors have been added yet");
            }

            return doctors.Select(x => x.ToDoctorDto()).ToList();
        }

        public List<GetDoctorDto> GetAllDoctorsBySpecialization(int userId, string specialization)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new Exception($"user with id {userId} not found");
            }

            var doctors = _repository.GetAll().Where(x => x.Specialization.ToString() == specialization);
            if (!doctors.Any())
            {
                throw new DoctorNotFoundException("a doctor with that specialization was not found");
            }
            return doctors.Select(x => x.ToDoctorDto()).ToList();
        }

        public GetDoctorDto GetDoctorById(int userId, int doctorId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new Exception($"user with id {userId} not found");
            }
            var doctor = _repository.GetById(doctorId);
            if (doctor == null)
            {
                throw new DoctorNotFoundException($"doctor with id {doctorId} not found");
            }

            return doctor.ToDoctorDto();
        }

        public void UpdateDoctor(DoctorDto doctorDto, int userId)
        {
            var doctor = _repository.GetAll().FirstOrDefault(x => x.UserId == userId);
            if (doctor == null)
            {
                throw new DoctorNotFoundException($"Doctor wih userId {userId} not found");
            }

            ValidateDoctor(doctorDto);
            var user = _userRepository.GetById(userId);

            doctor.Age = doctorDto.Age;
            doctor.FirstName = doctorDto.FirstName;
            doctor.LastName = doctorDto.LastName;
            doctor.Specialization = doctorDto.Specialization;

            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(doctorDto.Password));
            var hashedPassword = Encoding.ASCII.GetString(md5data);

            user.Password = hashedPassword;
            user.Email = doctorDto.Email;
            user.UserName = doctorDto.UserName;

            _userRepository.Update(user);
            _repository.Update(doctor);
        }


        private void ValidateDoctor(DoctorDto doctorDto)
        {
            Regex lowercaseRegex = new Regex("[a-z]");
            Regex specialCharRegex = new Regex("[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]");
            if (!lowercaseRegex.IsMatch(doctorDto.Password))
            {
                throw new Exception("Password must contain at least one lowercase letter.");
            }

            if (!specialCharRegex.IsMatch(doctorDto.Password))
            {
                throw new Exception("Password must contain at least one special character.");
            }

            if (string.IsNullOrEmpty(doctorDto.FirstName) || string.IsNullOrEmpty(doctorDto.LastName))
            {
                throw new ArgumentException("First and last name must not be empty or null");
            }
            if (doctorDto.Age <= 25 || doctorDto.Age >= 64)
            {
                throw new ArgumentException("Age must be between 25 and 64");
            }
            if (string.IsNullOrEmpty(doctorDto.Email) || !doctorDto.Email.Contains("@"))
            {
                throw new ArgumentException("Email should not be empty or null and should contain @");
            }
            if (string.IsNullOrEmpty(doctorDto.UserName))
            {
                throw new ArgumentException("Username must not be empty or null");
            }
            var existEmail = _userRepository.GetAll().FirstOrDefault(x => x.Email == doctorDto.Email);
            if (existEmail != null)
            {
                throw new Exception("Email already exists");
            }
            var existUserName = _userRepository.GetAll().FirstOrDefault(x => x.UserName == doctorDto.UserName);
            if (existUserName != null)
            {
                throw new Exception("Username already exists");
            }
        }
    }
}