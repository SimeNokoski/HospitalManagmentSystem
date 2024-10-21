using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.DTO.PatientDtos;
using HospitalManagementSystem.Services.Interfaces;
using HospitaManagmentSystem.Mapper;

namespace HospitalManagementSystem.Services.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IPatientsRepository _patientsRepository;
        public PatientService(IPatientsRepository patientsRepository)
        {
            _patientsRepository = patientsRepository;
        }

        public void add(PatientDto patientDto)
        {
            _patientsRepository.Add(patientDto.ToPatient());
        }
    }
}
