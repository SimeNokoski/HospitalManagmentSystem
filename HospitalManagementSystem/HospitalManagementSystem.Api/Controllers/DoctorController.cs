using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.DTO.DoctorDtos;
using HospitalManagementSystem.Services.Interfaces;
using HospitalManagementSystem.Shared;
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
                var userId = GetAuthorizedUserId();
                _doctor.CreateDoctor(doctorDto,userId);
                return Ok();
            }
            catch(UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("deleteDoctor/{id}"), Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult DeleteDoctor(int id)
        {
            try
            {
                var userId = GetAuthorizedUserId();
                _doctor.DeleteDoctor(id,userId);
                return Ok();
            }
            catch(UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch(DoctorNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }

        }

        [HttpPut("UpdateDoctor"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult UpdateDoctor(DoctorDto doctorDto)
        {
            try
            {
                var userId = GetAuthorizedUserId();
                _doctor.UpdateDoctor(doctorDto, userId);
                return Ok();
            }
            catch (DoctorNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetAllDoctors")]
        public IActionResult GetAllDoctors()
        {
            try
            {
                var userId = GetAuthorizedUserId();
                var doctors = _doctor.GetAllDoctor(userId);
                return Ok(doctors);
            }
            catch(DoctorNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetDoctorById/{id}")]
        public IActionResult GetDoctorById(int id)
        {
            try
            {
                var userId = GetAuthorizedUserId();
                var doctor = _doctor.GetDoctorById(userId,id);
                return Ok(doctor);
            }
            catch (DoctorNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetDoctorsBySpecialization/{specialization}")]
        public IActionResult GetDoctorsBySpecialization(string specialization)
        {
            try
            {
                var userId = GetAuthorizedUserId();
                var doctors = _doctor.GetAllDoctorsBySpecialization(userId,specialization);
                return Ok(doctors);
            }
            catch (DoctorNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
