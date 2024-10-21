using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.DataAccess.Implementation
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly HospitalManagementSystemDbContext _context;
        public MedicalRecordRepository(HospitalManagementSystemDbContext context)
        {
            _context = context;
        }
        public void Add(MedicalRecord entity)
        {
            _context.MedicalRecords.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(MedicalRecord entity)
        {
            _context.MedicalRecords.Remove(entity);
            _context.SaveChanges();
        }

        public List<MedicalRecord> GetAll()
        {
            return _context.MedicalRecords.Include(x=>x.Patients).Include(x=>x.Doctor).ToList();
        }

        public MedicalRecord GetById(int id)
        {
            return _context.MedicalRecords.FirstOrDefault(x => x.Id == id);
        }

        public void Update(MedicalRecord entity)
        {
            _context.MedicalRecords.Update(entity);
            _context.SaveChanges();
        }
    }
}
