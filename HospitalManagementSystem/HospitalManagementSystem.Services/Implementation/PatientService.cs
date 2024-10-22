using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.DTO.DoctorDtos;
using HospitalManagementSystem.DTO.PatientDtos;
using HospitalManagementSystem.Services.Interfaces;
using HospitalManagementSystem.Shared;
using HospitaManagmentSystem.Mapper;
using System.Text;
using System.Text.RegularExpressions;
using XSystem.Security.Cryptography;

namespace HospitalManagementSystem.Services.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IPatientsRepository _patientsRepository;
        private readonly IUserRepository _userRepository;
        public PatientService(IPatientsRepository patientsRepository, IUserRepository userRepository)
        {
            _patientsRepository = patientsRepository;
            _userRepository = userRepository;
        }

        public void DeletePatient(int userId, int id)
        {
            var user = _userRepository.GetById(userId);
            if(user == null)
            {
                throw new Exception($"user with id {userId} not found");
            }
            var patient = _patientsRepository.GetById(id);
            if(patient == null)
            {
                throw new PatientNotFoundException($"patient with id {id} not found");
            }
            _patientsRepository.Delete(patient);
            _userRepository.Delete(user);
        }

        public List<GetPatients> GetAllPatients(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new Exception("user with id {userId} not found");
            }

            var patients = _patientsRepository.GetAll();
            if(!patients.Any())
            {
                throw new PatientNotFoundException("no patients found in the database");
            }
            return patients.Select(x=>x.ToGetPatientDto()).ToList();
        }

        public GetPatients GetPatientById(int userId, int id)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new Exception("user with id {userId} not found");
            }
            var patient = _patientsRepository.GetById(id);
            if(patient == null)
            {
                throw new PatientNotFoundException($"patient with id {id} not found");
            }
            return patient.ToGetPatientDto();
        }

        public void UpdatePatient(int userId, PatientDto patientDto)
        {
            var patient = _patientsRepository.GetAll().FirstOrDefault(x=>x.UserId == userId);
            if (patient == null)
            {
                throw new PatientNotFoundException($"patient with userId {userId} not found");
            }

            ValidateDoctor(patientDto);

            var user = _userRepository.GetById(userId);

            patient.FirstName = patientDto.FirstName;
            patient.LastName = patientDto.LastName;
            patient.Age = patientDto.Age;

            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(patientDto.Password));
            var hashedPassword = Encoding.ASCII.GetString(md5data);

            user.Password = hashedPassword;
            user.Email = patientDto.Email;
            user.UserName = patientDto.UserName;

            _patientsRepository.Update(patient);
            _userRepository.Update(user);
        }



        private void ValidateDoctor(PatientDto patientDto)
        {
            Regex lowercaseRegex = new Regex("[a-z]");
            Regex specialCharRegex = new Regex("[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]");
            if (!lowercaseRegex.IsMatch(patientDto.Password))
            {
                throw new Exception("Password must contain at least one lowercase letter.");
            }

            if (!specialCharRegex.IsMatch(patientDto.Password))
            {
                throw new Exception("Password must contain at least one special character.");
            }

            if (string.IsNullOrEmpty(patientDto.FirstName) || string.IsNullOrEmpty(patientDto.LastName))
            {
                throw new ArgumentException("First and last name must not be empty or null");
            }
            if (patientDto.Age < 18 || patientDto.Age > 100)
            {
                throw new ArgumentException("Age must be between 18 and 100");
            }
            if (string.IsNullOrEmpty(patientDto.Email) || !patientDto.Email.Contains("@"))
            {
                throw new ArgumentException("Email should not be empty or null and should contain @");
            }
            if (string.IsNullOrEmpty(patientDto.UserName))
            {
                throw new ArgumentException("Username must not be empty or null");
            }
            var existEmail = _userRepository.GetAll().FirstOrDefault(x => x.Email == patientDto.Email);
            if (existEmail != null)
            {
                throw new Exception("Email already exists");
            }
            var existUserName = _userRepository.GetAll().FirstOrDefault(x => x.UserName == patientDto.UserName);
            if (existUserName != null)
            {
                throw new Exception("Username already exists");
            }
        }
    }
}
