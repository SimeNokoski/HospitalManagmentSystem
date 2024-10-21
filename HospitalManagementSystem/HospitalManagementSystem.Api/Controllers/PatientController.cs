using HospitalManagementSystem.DTO.PatientDtos;
using HospitalManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost("createPatient")]
        public IActionResult CreatePatient(PatientDto patientDto)
        {
            _patientService.add(patientDto);
            return Ok();
        }
    }
}
