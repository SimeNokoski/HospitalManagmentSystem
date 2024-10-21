using HospitalManagementSystem.DTO.MedicalRecordDtos;
using HospitalManagementSystem.Services.Interfaces;
using HospitalManagementSystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagementSystem.Api.Controllers
{
    [Authorize(Roles = "Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecord _medicalRecord;
        public MedicalRecordController(IMedicalRecord medicalRecord)
        {
            _medicalRecord = medicalRecord;
        }

        [HttpPost("createMedicalRecord")]
        public IActionResult CreateMedicalRecordForPatient(CreateMedicalRecordDto medicalRecordDto)
        {
            try
            {
                var userId = GetAuthorizedUserId();
                _medicalRecord.CreateMedicalRecordForPatient(medicalRecordDto, userId);
                return StatusCode(201, "CreateMedicalRecord");
            }
            catch (PatientNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }
        }

        [HttpGet("AllMedicalRecordByPatientById/id")]
       public IActionResult AllMedicalRecordByPatientId(int id)
        {
            try
            {
                var medicalRecord = _medicalRecord.AllMedicalRecordByPatientIds(id);
                return Ok(medicalRecord);
            }
            catch (PatientNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }

        }

        [HttpDelete("DeleteMedicalRecord/id")]
        public IActionResult DeleteMedicalRecord(int id)
        {
            try
            {
                var userId = GetAuthorizedUserId();
                _medicalRecord.DeleteMedicalRecord(id, userId);
                return Ok();
            }
            catch(UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (MedicalRecordNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "System error occurred, contact admin!");
            }

        }

        [HttpPut("UpdateMedicalRecord")]
        public IActionResult UpdateMedicalRecord(UpdateMedicalRecord updateMedicalRecord)
        {
            try
            {
                var userId = GetAuthorizedUserId();
                _medicalRecord.UpdateMedicalRecord(updateMedicalRecord, userId);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch(MedicalRecordNotFoundException ex)
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

        [HttpGet("GetAllMedicalRecord")]
        public IActionResult GetAllMedicalRecord()
        {
            try
            {
                var medicalRecords = _medicalRecord.GetAllMedicalRecord();
                return Ok(medicalRecords);
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
