using HospitalManagementSystem.DataAccess;
using HospitalManagementSystem.DataAccess.Implementation;
using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.Services.Services.Implementation;
using HospitalManagementSystem.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Helper
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<HospitalManagementSystemDbContext>(option => option.UseSqlServer(connectionString));
        }

        public static void InjectRepository(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPatientsRepository, PatientRepository>();
            services.AddTransient<IDoctorRepository, DoctorRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentsRepository>();
            services.AddTransient<IMedicalRecordRepository, MedicalRecordRepository>();
        }

        public static void InjectService(this IServiceCollection services)
        {
            services.AddTransient<IDoctorService, DoctorService>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IMedicalRecord, MedicalRecordService>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}