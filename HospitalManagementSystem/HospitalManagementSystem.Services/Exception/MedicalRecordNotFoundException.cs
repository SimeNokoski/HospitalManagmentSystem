namespace HospitalManagementSystem.Shared
{
    public class MedicalRecordNotFoundException : Exception
    {
        public MedicalRecordNotFoundException(string message) : base(message)
        {
        }
    }
}