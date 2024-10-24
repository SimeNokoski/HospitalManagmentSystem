namespace HospitalManagementSystem.Shared
{
    public class InvalidDateException : Exception
    {
        public InvalidDateException(string message) : base(message)
        {
        }
    }
}