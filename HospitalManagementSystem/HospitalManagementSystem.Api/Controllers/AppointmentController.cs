using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.DTO.AppointmentsDtos;
using HospitalManagementSystem.Services.Services.Interfaces;
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
            var doctorId = GetAuthorizedUserId();
            _appointmentService.AddAvailableAppointment(createAppointmentDto, doctorId);
            return Ok();
        }

        [HttpPut("bookAppointment"), Authorize(Roles = nameof(Role.Patient))]
        public IActionResult BookAppointment(BookCancelAppointmentDto bookAppointmentDto)
        {
            var patientId = GetAuthorizedUserId();
            _appointmentService.BookAppointment(bookAppointmentDto, patientId);
            return Ok();
        }

        [HttpGet("GetAppointmentsByDoctorId"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult GetAppointmentsByDoctorId()
        {
            var doctorId = GetAuthorizedUserId();
            var appointments = _appointmentService.GetAppointmentsByDoctorId(doctorId);
            return Ok(appointments);
        }

        [HttpGet("AvailableAppointmentsByDoctorId/{id}"), Authorize(Roles = nameof(Role.Patient))]
        public IActionResult AvailableAppointmentsByDoctorId(int id)
        {
            var availableAppointments = _appointmentService.AvailableAppointmentsByDoctorId(id);
            return Ok(availableAppointments);
        }

        [HttpDelete("deleteAppointment/{id}"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult DeleteAppointment(int id)
        {
            var userId = GetAuthorizedUserId();
            _appointmentService.RemoveAppointment(id, userId);
            return Ok();
        }

        [HttpPut("cancelAppointment"), Authorize(Roles = nameof(Role.Patient))]
        public IActionResult CancelAppointment(BookCancelAppointmentDto bookCancelAppointmentDto)
        {
            var patientId = GetAuthorizedUserId();
            _appointmentService.CancelAppointment(bookCancelAppointmentDto, patientId);
            return Ok();
        }

        [HttpGet("allPatientByDoctor"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult AllPatientByDoctorId()
        {
            var doctorId = GetAuthorizedUserId();
            var allpatient = _appointmentService.AllPatientByDoctorId(doctorId);
            return Ok(allpatient);
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