using HospitalManagementSystem.DTO.MedicalRecordDtos;

namespace HospitalManagementSystem.Services.Interfaces
{
    public interface IMedicalRecord
    {
        void CreateMedicalRecordForPatient(CreateMedicalRecordDto medicalRecordDto, int userId);
        List<MedicalRecordDto> AllMedicalRecordByPatientIds(int patientId);
        void DeleteMedicalRecord(int id, int userId);
        void UpdateMedicalRecord(UpdateMedicalRecord updateMedicalRecord, int userId);
        List<MedicalRecordDto> GetAllMedicalRecord();
    }
}
