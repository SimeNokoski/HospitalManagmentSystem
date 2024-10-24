namespace HospitalManagementSystem.Shared
{
    public class DoctorNotFoundException : Exception
    {
        public DoctorNotFoundException(string message) : base(message)
        {
        }
    }
}