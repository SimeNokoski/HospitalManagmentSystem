using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.DTO.MedicalRecordDtos;
using HospitalManagementSystem.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecord _medicalRecord;

        public MedicalRecordController(IMedicalRecord medicalRecord)
        {
            _medicalRecord = medicalRecord;
        }

        [HttpPost("createMedicalRecord"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult CreateMedicalRecordForPatient(CreateMedicalRecordDto medicalRecordDto)
        {
            var userId = GetAuthorizedUserId();
            _medicalRecord.CreateMedicalRecordForPatient(medicalRecordDto, userId);
            return StatusCode(201, "CreateMedicalRecord");
        }

        [HttpGet("AllMedicalRecordByPatientById/{id}"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult AllMedicalRecordByPatientId(int id)
        {
            var medicalRecord = _medicalRecord.AllMedicalRecordByPatientIds(id);
            return Ok(medicalRecord);
        }

        [HttpDelete("DeleteMedicalRecord/id"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult DeleteMedicalRecord(int id)
        {
            var userId = GetAuthorizedUserId();
            _medicalRecord.DeleteMedicalRecord(id, userId);
            return Ok();
        }

        [HttpPut("UpdateMedicalRecord"), Authorize(Roles = nameof(Role.Doctor))]
        public IActionResult UpdateMedicalRecord(UpdateMedicalRecord updateMedicalRecord)
        {
            var userId = GetAuthorizedUserId();
            _medicalRecord.UpdateMedicalRecord(updateMedicalRecord, userId);
            return Ok();
        }

        [HttpGet("GetAllMedicalRecord"), Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult GetAllMedicalRecord()
        {
            var userId = GetAuthorizedUserId();
            var medicalRecords = _medicalRecord.GetAllMedicalRecord(userId);
            return Ok(medicalRecords);
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