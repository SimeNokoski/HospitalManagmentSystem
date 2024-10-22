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
        private readonly IUserRepository _userRepository;
        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository, IDoctorRepository doctorRepository, IPatientsRepository petientsRepository, IUserRepository userRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _doctorRepository = doctorRepository;
            _petientsRepository = petientsRepository;
            _userRepository = userRepository;
        }

        public List<MedicalRecordDto> AllMedicalRecordByPatientIds(int patientId)
        {
           var patient = _petientsRepository.GetById(patientId);
            if(patient == null)
            {
                throw new PatientNotFoundException($"Patient with id: {patientId} not found"); 
            }
            var medicalRecords = _medicalRecordRepository.GetAll().Where(x => x.PatientId == patientId);
            if(!medicalRecords.Any())
            {
                throw new MedicalRecordNotFoundException("there is no medical record for that patient");
            }
            return medicalRecords.Select(x=>x.ToMedicalRecord()).ToList();
        }

        public void CreateMedicalRecordForPatient(CreateMedicalRecordDto medicalRecordDto, int userId)
        {
            var doctor = _doctorRepository.GetAll().FirstOrDefault(x=>x.UserId == userId);
            if(doctor == null)
            {
                throw new DoctorNotFoundException($"doctor with userid {userId} not found");
            }
            if(medicalRecordDto.DateTime < DateTime.UtcNow || medicalRecordDto.StartDate <DateTime.UtcNow)
            {
                throw new InvalidDataException("time cannot be in the past tense");
            }
            if(medicalRecordDto.StartDate > medicalRecordDto.EndDate)
            {
                throw new InvalidDataException("the start time must be greater than the end time");
            }
            if(medicalRecordDto.StartDate < medicalRecordDto.DateTime)
            {
                throw new InvalidDataException("the record time must not be greater than the start time");
            }
            var patient = _petientsRepository.GetById(medicalRecordDto.PatientId);
            if(patient == null)
            {
                throw new PatientNotFoundException($"patient with id {medicalRecordDto.PatientId} not found");
            }

            var medicalReocord = medicalRecordDto.ToMedicalRecord();
            medicalReocord.DoctorId = doctor.Id;
            _medicalRecordRepository.Add(medicalReocord);
        }

        public void DeleteMedicalRecord(int id,int userId)
        {
            var doctor = _doctorRepository.GetAll().FirstOrDefault(x=>x.UserId==userId);
            if (doctor == null)
            {
                throw new DoctorNotFoundException($"doctor with userid {userId} not found");
            }
            var medicalRecord = _medicalRecordRepository.GetById(id);
            if (medicalRecord == null)
            {
                throw new MedicalRecordNotFoundException($"Medical record with id {id} does not exist");
            }
            if (medicalRecord.DoctorId != doctor.Id)
            {
                throw new Exception("You do not have permission to delete this medical record");
            }
  
            _medicalRecordRepository.Delete(medicalRecord);
        }

        public List<MedicalRecordDto> GetAllMedicalRecord(int userId)
        {
            var user = _userRepository.GetById(userId);
            if(user == null)
            {
                throw new Exception("user with id {userId} not found");
            }
            var medicalRecords = _medicalRecordRepository.GetAll();
            if(medicalRecords == null)
            {
                throw new MedicalRecordNotFoundException("no medical records");
            }
            return medicalRecords.Select(x=>x.ToMedicalRecord()).ToList();
        }

        public void UpdateMedicalRecord(UpdateMedicalRecord updateMedicalRecord, int userId)
        {
            var doctor = _doctorRepository.GetAll().FirstOrDefault(x=>x.UserId == userId);
            if (doctor == null)
            {
                throw new DoctorNotFoundException($"doctor with userid {userId} not found");
            }
            var medicalRecord = _medicalRecordRepository.GetById(updateMedicalRecord.Id);
            if (medicalRecord == null)
            {
                throw new MedicalRecordNotFoundException($"Medical record with id {medicalRecord.Id} does not exist");
            }
            if (medicalRecord.DoctorId != doctor.Id)
            {
                throw new Exception("You do not have permission to delete this medical record");
            }
          
            if (updateMedicalRecord.StartDate > updateMedicalRecord.EndDate)
            {
                throw new InvalidDataException("the start time must be greater than the end time");
            }
            if(updateMedicalRecord.StartDate < DateTime.UtcNow)
            {
                throw new InvalidDataException("time cannot be in the past tense");
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
