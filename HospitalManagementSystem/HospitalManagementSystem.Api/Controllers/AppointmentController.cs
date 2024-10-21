using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.DTO.AppointmentsDtos;
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
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("createAppointment"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult CreateAppointment(CreateAppointmentDto createAppointmentDto)
        {
            try
            {
                var doctorId = GetAuthorizedUserId();
                _appointmentService.AddAvailableAppointment(createAppointmentDto, doctorId);
                return Ok();
            }
            catch(DoctorNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }
        }

        [HttpPut("bookAppointment"), Authorize(Roles = nameof(Role.Patient))]
        public IActionResult BookAppointment(BookAppointmentDto bookAppointmentDto)
        {
            try
            {
                var patientId = GetAuthorizedUserId();
                _appointmentService.BookAppointment(bookAppointmentDto, patientId);
                return Ok();
            }
            catch (PatientNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(AppointmentNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }
        }

        [HttpGet("GetAppointmentsByDoctorId"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult GetAppointmentsByDoctorId()
        {
            try
            {
                var doctorId = GetAuthorizedUserId();
                var appointments = _appointmentService.GetAppointmentsByDoctorId(doctorId);
                return Ok(appointments);
            }
            catch (DoctorNotFoundException ex)
            {
               return NotFound(ex.Message);
            }
            catch (AppointmentNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }

        }

        [HttpGet("AvailableAppointmentsByDoctorId"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult AvailableAppointmentsByDoctorId()
        {
            try
            {
                var userId = GetAuthorizedUserId();
                var availableAppointments = _appointmentService.AvailableAppointmentsByDoctorId(userId);
                return Ok(availableAppointments);
            }
            catch (DoctorNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(AppointmentNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }

        }

        [HttpDelete("deleteAppointment/id"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult DeleteAppointment(int id)
        {
            try
            {
                var userId = GetAuthorizedUserId();
                _appointmentService.RemoveAppointment(id, userId);
                return Ok();
            }
            catch (DoctorNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AppointmentNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }
        }

        [HttpPut("cancelAppointment")]
        public IActionResult CancelAppointment(PatientCancelAppointmentDto patientCancelAppointmentDto)
        {
            try
            {
                var patientId = GetAuthorizedUserId();
                _appointmentService.CancelAppointment(patientCancelAppointmentDto, patientId);
                return Ok();
            }
            catch (PatientNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AppointmentNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }

        }

        [HttpGet("allPatientByDoctor"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult AllPatientByDoctorId()
        {
            try
            {
                var doctorId = GetAuthorizedUserId();
                var allpatient = _appointmentService.AllPatientByDoctorId(doctorId);
                return Ok(allpatient);
            }
            catch (DoctorNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(PatientNotFoundException ex)
            {
                return NotFound(ex.Message);
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
