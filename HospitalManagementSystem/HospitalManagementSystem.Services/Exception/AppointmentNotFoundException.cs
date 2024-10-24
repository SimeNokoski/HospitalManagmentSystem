namespace HospitalManagementSystem.Shared
{
    public class AppointmentNotFoundException : Exception
    {
        public AppointmentNotFoundException(string message) : base(message)
        {
        }
    }
}