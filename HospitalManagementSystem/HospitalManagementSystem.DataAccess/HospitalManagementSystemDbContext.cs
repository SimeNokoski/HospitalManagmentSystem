using HospitalManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using XSystem.Security.Cryptography;

namespace HospitalManagementSystem.DataAccess
{
    public class HospitalManagementSystemDbContext : DbContext
    {
        public HospitalManagementSystemDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patients> Patients { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }    
        public DbSet<Appointments> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointments>()
                     .HasOne(a => a.Patient)
                     .WithMany(p => p.Appointments)
                     .HasForeignKey(a => a.PatientId)
                     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Patients)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Doctor)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>()
                 .HasOne(d => d.User)
                 .WithOne(d=>d.Doctor)
                 .HasForeignKey<Doctor>(d => d.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Patients>()
                .HasOne(p => p.User)
                .WithOne(p=>p.Patients)
                .HasForeignKey<Patients>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);



            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes("sime123!"));
            var hashedPassword = Encoding.ASCII.GetString(md5data);

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                   Id = 1,
                   Email = "nokoski99@gmail.com",
                   Password = hashedPassword,
                   Role = Domain.Enums.Role.SuperAdmin,
                   UserName = "simeSuperAdmin"               
                });
        }
    }
}
