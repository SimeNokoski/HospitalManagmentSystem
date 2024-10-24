namespace HospitalManagementSystem.Shared
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException(string message) : base(message)
        {
        }
    }
}