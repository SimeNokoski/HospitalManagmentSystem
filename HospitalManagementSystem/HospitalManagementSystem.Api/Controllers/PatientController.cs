using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.DTO.PatientDtos;
using HospitalManagementSystem.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("GetAllPatients"), Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult GetAllPatients()
        {
            var userId = GetAuthorizedUserId();
            var patients = _patientService.GetAllPatients(userId);
            return Ok(patients);
        }

        [HttpGet("GetPatientsById/{id}"), Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult GetPatientsById(int id)
        {
            var userId = GetAuthorizedUserId();
            var patient = _patientService.GetPatientById(userId, id);
            return Ok(patient);
        }

        [HttpPost("UpdatePatient"), Authorize(Roles = nameof(Role.Patient))]
        public IActionResult UpdatePatient(PatientDto patientDto)
        {
            var userId = GetAuthorizedUserId();
            _patientService.UpdatePatient(userId, patientDto);
            return Ok();
        }

        [HttpDelete("DeletePatient/{id}"), Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult DeletePatient(int id)
        {
            _patientService.DeletePatient(id);
            return Ok();
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