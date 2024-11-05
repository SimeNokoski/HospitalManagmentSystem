using AutoMapper;
using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.Domain.Models;
using HospitalManagementSystem.DTO.AppointmentsDtos;
using HospitalManagementSystem.DTO.DoctorDtos;
using HospitalManagementSystem.DTO.MedicalRecordDtos;
using HospitalManagementSystem.DTO.PatientDtos;
using HospitalManagementSystem.DTO.UserDtos;

namespace HospitalManagementSystem.Services.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Appointments, BookCancelAppointmentDto>().ReverseMap();
            CreateMap<Appointments, CreateAppointmentDto>().ReverseMap();
            CreateMap<Appointments, GetAppointmentsByDoctorId>()
                .ForMember(dest => dest.DoctorFullName,
               opt => opt.MapFrom(src => $"{src.Doctor.FirstName} {src.Doctor.LastName}"));
            CreateMap<Appointments, AllPatientByDtoctorIdDto>()
               .ForMember(dest => dest.FullName,
               opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"));

            CreateMap<Doctor, GetDoctorDto>()
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email)).ReverseMap();

            CreateMap<DoctorDto, Doctor>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User
            {
                Email = src.Email,
                Password = "",
                UserName = src.UserName,
                Role = Role.Doctor
            }))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ReverseMap();

            CreateMap<MedicalRecord, CreateMedicalRecordDto>().ReverseMap();
            CreateMap<MedicalRecord, MedicalRecordDto>()
            .ForMember(dest => dest.PatientFullName, opt => opt.MapFrom(src => $"{src.Patients.FirstName} {src.Patients.LastName}"))
            .ForMember(dest => dest.DoctorFullName, opt => opt.MapFrom(src => $"{src.Doctor.FirstName} {src.Doctor.LastName}"))
            .ReverseMap();
            CreateMap<MedicalRecord, UpdateMedicalRecord>().ReverseMap();

            CreateMap<Patients, GetPatients>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))          
            .ReverseMap();

            CreateMap<Patients, PatientDto>().ReverseMap();

            CreateMap<User, LoginUserDto>().ReverseMap();
            CreateMap<User, RegisterUserDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();



        }
    }
}