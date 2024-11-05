using AutoMapper;
using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.Domain.Models;
using HospitalManagementSystem.DTO.AppointmentsDtos;
using HospitalManagementSystem.Services.Services.Interfaces;
using HospitalManagementSystem.Shared;

namespace HospitalManagementSystem.Services.Services.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IPatientsRepository _patientsRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentsRepository, IPatientsRepository patientsRepository, IDoctorRepository doctorRepository, IMapper mapper)
        {
            _appointmentsRepository = appointmentsRepository;
            _patientsRepository = patientsRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public void AddAvailableAppointment(CreateAppointmentDto createAppointmentDto, int userId)
        {
            var doctor = _doctorRepository.GetAll().FirstOrDefault(x => x.UserId == userId);
            if (doctor == null)
            {
                throw new DoctorNotFoundException($"Doctor wih userId {userId} not found");
            }
            if (createAppointmentDto.DateTime < DateTime.UtcNow)
            {
                throw new InvalidDataException("The appointment time cannot be in the past tense");
            }
            bool isAppointmentExists = _appointmentsRepository.GetAll().Any(x => x.DoctorId == doctor.Id && x.DateTime == createAppointmentDto.DateTime);
            if (isAppointmentExists)
            {
                throw new InvalidDataException("There is already an appointment at the specified time");
            }

            var appointment = _mapper.Map<Appointments>(createAppointmentDto);
            appointment.DoctorId = doctor.Id;
            _appointmentsRepository.Add(appointment);
        }

        public List<AllPatientByDtoctorIdDto> AllPatientByDoctorId(int userId)
        {
            var doctor = _doctorRepository.GetAll().FirstOrDefault(x => x.UserId == userId);
            if (doctor == null)
            {
                throw new DoctorNotFoundException($"Doctor wih userId {userId} not found");
            }

            var patients = _appointmentsRepository.GetAll().Where(x => x.DoctorId == doctor.Id && x.PatientId != null).ToList();
            if (!patients.Any())
            {
                throw new PatientNotFoundException($"No patients found for doctor with id {doctor.Id}");
            }

            return _mapper.Map<List<AllPatientByDtoctorIdDto>>(patients);
        }

        public List<GetAppointmentsByDoctorId> AvailableAppointmentsByDoctorId(int doctorId)
        {
            var doctor = _doctorRepository.GetById(doctorId);
            if (doctor == null)
            {
                throw new DoctorNotFoundException($"Doctor wih id {doctorId} not found");
            }
            var appointmets = _appointmentsRepository.GetAll().Where(x => !x.Status && x.DoctorId == doctor.Id && x.DateTime > DateTime.UtcNow).ToList();
            if (!appointmets.Any())
            {
                throw new AppointmentNotFoundException($"No available appointments found for doctor with id {doctor.Id}");
            }

            return _mapper.Map<List<GetAppointmentsByDoctorId>>(appointmets);
        }

        public void BookAppointment(BookCancelAppointmentDto bookAppointmentDto, int userId)
        {
            var patient = _patientsRepository.GetAll().FirstOrDefault(x => x.UserId == userId);
            if (patient == null)
            {
                throw new PatientNotFoundException($"Patient with userid {userId} not found");
            }
            var appointment = _appointmentsRepository.GetById(bookAppointmentDto.AppointmentId);
            if (appointment == null)
            {
                throw new AppointmentNotFoundException($"Appointment with id {appointment.Id} not found");
            }
            if (appointment.Status)
            {
                throw new InvalidOperationException("The appointment is already booked");
            }
            appointment.Status = true;
            appointment.PatientId = patient.Id;

            _appointmentsRepository.Update(appointment);
        }

        public void CancelAppointment(BookCancelAppointmentDto bookCancelAppointmentDto, int userId)
        {
            var patient = _patientsRepository.GetAll().FirstOrDefault(x => x.UserId == userId);
            if (patient == null)
            {
                throw new PatientNotFoundException($"Patient with userid {userId} not found");
            }
            var appointment = _appointmentsRepository.GetById(bookCancelAppointmentDto.AppointmentId);
            if (appointment == null)
            {
                throw new AppointmentNotFoundException($"Appointment with id {appointment.Id} not found");
            }
            if (appointment.PatientId != patient.Id)
            {
                throw new UnauthorizedAccessException("Do not cancel this appointment");
            }
            appointment.Status = false;
            appointment.PatientId = null;
            _appointmentsRepository.Update(appointment);
        }

        public List<GetAppointmentsByDoctorId> GetAppointmentsByDoctorId(int userId)
        {
            var doctor = _doctorRepository.GetAll().FirstOrDefault(x => x.UserId == userId);
            if (doctor == null)
            {
                throw new DoctorNotFoundException($"Doctor wih userId {userId} not found");
            }
            var appointments = _appointmentsRepository.GetAll().Where(x => x.DoctorId == doctor.Id).ToList();
            if (!appointments.Any())
            {
                throw new AppointmentNotFoundException($"No appointments found for doctor with id {doctor.Id}");
            }
            return _mapper.Map<List<GetAppointmentsByDoctorId>>(appointments);
        }

        public void RemoveAppointment(int id, int userId)
        {
            var doctor = _doctorRepository.GetAll().FirstOrDefault(x => x.UserId == userId);
            if (doctor == null)
            {
                throw new DoctorNotFoundException($"Doctor wih userId {userId} not found");
            }
            var appointment = _appointmentsRepository.GetById(id);
            if (appointment == null)
            {
                throw new AppointmentNotFoundException($"Appointment with id {appointment.Id} not found");
            }
            if (appointment.DoctorId != doctor.Id)
            {
                throw new UnauthorizedAccessException("You do not have permission to remove this appointment");
            }
            _appointmentsRepository.Delete(appointment);
        }
    }
}