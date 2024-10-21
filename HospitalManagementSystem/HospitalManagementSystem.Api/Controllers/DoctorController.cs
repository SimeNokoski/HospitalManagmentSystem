using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.DTO.DoctorDtos;
using HospitalManagementSystem.Services.Interfaces;
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
            try
            {
                _doctor.CreateDoctor(doctorDto);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }
        }

        [HttpDelete("deleteDoctor/id"), Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult DeleteDoctor(int id)
        {
            try
            {
                _doctor.DeleteDoctor(id);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }

        }

        [HttpPut("UpdateDoctor"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult UpdateDoctor(UpdateDoctorDto updateDoctorDto)
        {
            try
            {
                var userId = GetAuthorizedUserId();
                _doctor.UpdateDoctor(updateDoctorDto,userId);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }
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
