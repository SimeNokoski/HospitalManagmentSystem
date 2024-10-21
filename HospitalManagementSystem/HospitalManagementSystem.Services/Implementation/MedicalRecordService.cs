using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.Domain.Models;
using HospitalManagementSystem.DTO.MedicalRecordDtos;
using HospitalManagementSystem.Services.Interfaces;
using HospitalManagementSystem.Shared;
using HospitaManagmentSystem.Mapper;

namespace HospitalManagementSystem.Services.Implementation
{
    public class MedicalRecordService : IMedicalRecord
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientsRepository _petientsRepository;
        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository, IDoctorRepository doctorRepository, IPatientsRepository petientsRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _doctorRepository = doctorRepository;
            _petientsRepository = petientsRepository;
        }

        public List<MedicalRecordDto> AllMedicalRecordByPatientIds(int patientId)
        {
           var patient = _petientsRepository.GetById(patientId);
            if(patient == null)
            {
                throw new PatientNotFoundException($"Patient with id: {patientId} not found"); 
            }
            var medicalRecords = _medicalRecordRepository.GetAll().Where(x => x.PatientId == patientId);
            return medicalRecords.Select(x=>x.ToMedicalRecord()).ToList();
        }

        public void CreateMedicalRecordForPatient(CreateMedicalRecordDto medicalRecordDto, int userId)
        {
            var doctor = _doctorRepository.GetAll().FirstOrDefault(x=>x.UserId == userId);
            if(medicalRecordDto.PatientId == null)
            {
                throw new PatientNotFoundException($"Patient with id: {medicalRecordDto.PatientId} not found");
            }
            var medicalReocord = medicalRecordDto.ToMedicalRecord();
            medicalReocord.DoctorId = doctor.Id;
            _medicalRecordRepository.Add(medicalReocord);
        }

        public void DeleteMedicalRecord(int id,int userId)
        {
            var doctor = _doctorRepository.GetAll().FirstOrDefault(x=>x.UserId==userId);
            var medicalRecord = _medicalRecordRepository.GetById(id);
            if (medicalRecord.DoctorId != doctor.Id)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this medical record");
            }

            if (medicalRecord == null)
            {
                throw new MedicalRecordNotFoundException($"Medical record with id {id} does not exist");
            }   
            _medicalRecordRepository.Delete(medicalRecord);
        }

        public List<MedicalRecordDto> GetAllMedicalRecord()
        {
            var medicalRecords = _medicalRecordRepository.GetAll();
            return medicalRecords.Select(x=>x.ToMedicalRecord()).ToList();
        }

        public void UpdateMedicalRecord(UpdateMedicalRecord updateMedicalRecord, int userId)
        {
            var doctor = _doctorRepository.GetAll().FirstOrDefault(x=>x.UserId == userId);
            var medicalRecord = _medicalRecordRepository.GetById(updateMedicalRecord.Id);

            if (medicalRecord.DoctorId != doctor.Id)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this medical record");
            }

            if (medicalRecord == null)
            {
                throw new MedicalRecordNotFoundException($"Medical record with id {medicalRecord.Id} does not exist");
            }
            if (updateMedicalRecord.StartDate < DateTime.UtcNow)
            {
                throw new InvalidDataException("Invalid start date");
            }
            if (updateMedicalRecord.EndDate < updateMedicalRecord.StartDate)
            {
                throw new InvalidDataException("invalid end date");
            }


            medicalRecord.Description = updateMedicalRecord.Description;
            medicalRecord.StartDate = updateMedicalRecord.StartDate;
            medicalRecord.EndDate = updateMedicalRecord.EndDate;
            medicalRecord.Treatment = updateMedicalRecord.Description;
            medicalRecord.Diagnosis = updateMedicalRecord.Diagnosis;
            medicalRecord.DoctorId = doctor.Id;
       
            _medicalRecordRepository.Update(medicalRecord);
        }
    }
}
