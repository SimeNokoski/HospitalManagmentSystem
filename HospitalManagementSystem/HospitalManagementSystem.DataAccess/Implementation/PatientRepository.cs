using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.Domain.Models;

namespace HospitalManagementSystem.DataAccess.Implementation
{
    public class PatientRepository : IPatientsRepository
    {
        private readonly HospitalManagementSystemDbContext _context;
        public PatientRepository(HospitalManagementSystemDbContext context)
        {
            _context = context;
        }
        public void Add(Patients entity)
        {
           _context.Patients.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Patients entity)
        {
            _context.Patients.Remove(entity);
            _context.SaveChanges();
        }

        public List<Patients> GetAll()
        {
            return _context.Patients.ToList();
        }

        public Patients GetById(int id)
        {
            return _context.Patients.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Patients entity)
        {
            _context.Patients.Update(entity);
            _context.SaveChanges();
        }
    }
}
