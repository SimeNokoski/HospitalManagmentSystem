using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.DataAccess.Implementation
{
    public class AppointmentsRepository : IAppointmentRepository
    {
        private readonly HospitalManagementSystemDbContext _context;
        public AppointmentsRepository(HospitalManagementSystemDbContext context)
        {
            _context = context;
        }

        public void Add(Appointments entity)
        {
            _context.Appointments.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Appointments entity)
        {
            _context.Appointments.Remove(entity);
            _context.SaveChanges();
        }

        public List<Appointments> GetAll()
        {
            return _context.Appointments.Include(x=>x.Doctor).Include(x=>x.Patient).ToList();
        }

        public Appointments GetById(int id)
        {
            return _context.Appointments.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Appointments entity)
        {
            _context.Appointments.Update(entity);
            _context.SaveChanges();
        }
    }
}
