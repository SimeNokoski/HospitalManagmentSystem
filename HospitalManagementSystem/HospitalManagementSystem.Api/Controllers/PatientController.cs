using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.DTO.PatientDtos;
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
            try
            {
                var userId = GetAuthorizedUserId();
                var patients = _patientService.GetAllPatients(userId);
                return Ok(patients);
            }
            catch(PatientNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetPatientsById/{id}"), Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult GetPatientsById(int id)
        {
            try
            {
                var userId = GetAuthorizedUserId();
                var patient = _patientService.GetPatientById(userId, id);
                return Ok(patient);
            }
            catch (PatientNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("UpdatePatient"), Authorize(Roles = nameof(Role.Patient))]
        public IActionResult UpdatePatient(PatientDto patientDto)
        {
            try
            {
                var userId = GetAuthorizedUserId();
                _patientService.UpdatePatient(userId, patientDto);
                return Ok();
            }
            catch (PatientNotFoundException ex)
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

        [HttpDelete("DeletePatient/{id}"), Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult DeletePatient(int id)
        {
            try
            {
                var userId = GetAuthorizedUserId();
                _patientService.DeletePatient(userId, id);
                return Ok();
            }
            catch (PatientNotFoundException ex)
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
