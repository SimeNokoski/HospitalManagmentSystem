using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.DataAccess.Implementation
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HospitalManagementSystemDbContext _context;
        public DoctorRepository(HospitalManagementSystemDbContext context)
        {
            _context = context;
        }

        public void Add(Doctor entity)
        {
            _context.Doctors.Add(entity);   
            _context.SaveChanges();
        }

        public void Delete(Doctor entity)
        {
            _context.Doctors.Remove(entity);
            _context.SaveChanges();
        }

        public List<Doctor> GetAll()
        {
            return _context.Doctors.Include(x=>x.User).ToList();
        }

        public Doctor GetById(int id)
        {
            return _context.Doctors.Include(x=>x.User).FirstOrDefault(x => x.Id == id);
        }

        public void Update(Doctor entity)
        {
            _context.Doctors.Update(entity);
            _context.SaveChanges();
        }
    }
}
