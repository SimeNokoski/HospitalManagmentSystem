using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.DTO.DoctorDtos;
using HospitalManagementSystem.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctor;

        public DoctorController(IDoctorService doctor)
        {
            _doctor = doctor;
        }

        [HttpPost("createDoctor"), Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult AdddDoctor(DoctorDto doctorDto)
        {
            var userId = GetAuthorizedUserId();
            _doctor.CreateDoctor(doctorDto, userId);
            return Ok();
        }

        [HttpDelete("deleteDoctor/{id}"), Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult DeleteDoctor(int id)
        {
            _doctor.DeleteDoctor(id);
            return Ok();
        }

        [HttpPut("UpdateDoctor"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult UpdateDoctor(DoctorDto doctorDto)
        {
            var userId = GetAuthorizedUserId();
            _doctor.UpdateDoctor(doctorDto, userId);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("GetAllDoctors")]
        public IActionResult GetAllDoctors()
        {
            var userId = GetAuthorizedUserId();
            var doctors = _doctor.GetAllDoctor(userId);
            return Ok(doctors);
        }

        [AllowAnonymous]
        [HttpGet("GetDoctorById/{id}")]
        public IActionResult GetDoctorById(int id)
        {
            var userId = GetAuthorizedUserId();
            var doctor = _doctor.GetDoctorById(userId, id);
            return Ok(doctor);
        }

        [AllowAnonymous]
        [HttpGet("GetDoctorsBySpecialization/{specialization}")]
        public IActionResult GetDoctorsBySpecialization(string specialization)
        {
            var userId = GetAuthorizedUserId();
            var doctors = _doctor.GetAllDoctorsBySpecialization(userId, specialization);
            return Ok(doctors);
        }

        private int GetAuthorizedUserId()
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?
                .Value, out var userId))
            {
                string name = User.FindFirst(ClaimTypes.Name)?.Value;
                throw new Exception($"{name} identifier claim does not exist!");
            }
            return userId;
        }
    }
}