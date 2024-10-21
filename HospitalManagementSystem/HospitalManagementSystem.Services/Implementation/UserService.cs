using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.Domain.Models;
using HospitalManagementSystem.DTO.UserDtos;
using HospitalManagementSystem.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using XSystem.Security.Cryptography;
using User = HospitalManagementSystem.Domain.Models.User;

namespace HospitalManagementSystem.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientsRepository _petientRepository;
        public UserService(IUserRepository userRepository, IDoctorRepository doctorRepository, IPatientsRepository patientsRepository)
        {
            _userRepository = userRepository;
            _doctorRepository = doctorRepository;
            _petientRepository = patientsRepository;
        }
        public string LoginUser(LoginUserDto loginUserDto)
        {
            if (string.IsNullOrEmpty(loginUserDto.Username) || string.IsNullOrEmpty(loginUserDto.Password))
            {
                throw new Exception("Username and password are required fields!");
            }

            // hash the password
            //MD5 hash algorithm
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();

            //Test123 -> 5467821
            byte[] passwordBytes = Encoding.ASCII.GetBytes(loginUserDto.Password);

            //get the bytes of the hash string 5467821 -> 2363621
            byte[] hashBytes = mD5CryptoServiceProvider.ComputeHash(passwordBytes);

            //get the hash as string 2363621 -> q654klj
            string hash = Encoding.ASCII.GetString(hashBytes);

            //try to get the user
            User userDb = _userRepository.LoginUser(loginUserDto.Username, hash);
            if (userDb == null)
            {
                throw new Exception("User not found");
            }

            //GENERATE JWT TOKEN
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] secretKeyBytes = Encoding.ASCII.GetBytes("Our secret secret secret secret secret secret key");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(1), // the token will be valid for one min
                //signature configuration
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature),
                //payload
                Subject = new ClaimsIdentity(
                    new[]
                   {
                        new Claim(ClaimTypes.NameIdentifier, userDb.Id.ToString()),
                        new Claim(ClaimTypes.Name, userDb.UserName),
                        new Claim(ClaimTypes.Role, userDb.Role.ToString())
                    }
                )
            };

            //generate token
            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            //convert to string
            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public void Register(RegisterUserDto registerUserDto)
        {
            string password = registerUserDto.Password;

            // Define regular expressions for lowercase letters and special characters
            Regex lowercaseRegex = new Regex("[a-z]");
            Regex specialCharRegex = new Regex("[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]");

            // Check if the password contains at least one lowercase letter
            if (!lowercaseRegex.IsMatch(password))
            {
                throw new Exception("Password must contain at least one lowercase letter.");
            }

            // Check if the password contains at least one special character
            if (!specialCharRegex.IsMatch(password))
            {
                throw new Exception("Password must contain at least one special character.");
            }

            //validate user
            ValidateUser(registerUserDto);

            //hash the password
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();

            //Test123 -> 546721
            byte[] passwordBytes = Encoding.ASCII.GetBytes(registerUserDto.Password);

            //get the bytes of hash string 546721 -> 21346
            byte[] hashBytes = mD5CryptoServiceProvider.ComputeHash(passwordBytes);

            //get the has as string 21346-> qR5Tf
            string hash = Encoding.ASCII.GetString(hashBytes);

            //create the user
            User user = new User
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.UserName,
                Role = Domain.Enums.Role.Patient,
                Password = hash,
            };
            _userRepository.Add(user);         

            if(user.Role == Domain.Enums.Role.Patient)
            {
                Patients patients = new Patients
                {
                    Age = registerUserDto.Age,
                    FirstName = registerUserDto.FirstName,
                    LastName = registerUserDto.LastName,
                    UserId= user.Id,
                };
                _petientRepository.Add(patients);
            }
           
        }


        public void ValidateUser(RegisterUserDto registerUserDto)
        {
            if (string.IsNullOrEmpty(registerUserDto.UserName) || string.IsNullOrEmpty(registerUserDto.Password))
            {
                throw new Exception("Username and password are required!");
            }

            if (registerUserDto.UserName.Length > 50)
            {
                throw new Exception("Maximum length of username is 50 characters");
            }

            if (registerUserDto.Password != registerUserDto.ConfirmPassword)
            {
                throw new Exception("Passwords must match");
            }

            var userDb = _userRepository.GetUserByUsername(registerUserDto.UserName);
            if (userDb != null) //if there is a user with that username in db
            {
                throw new Exception($"The username {registerUserDto.UserName} is already taken!");
            }
        }
    }
}
