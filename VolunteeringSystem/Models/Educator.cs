namespace VolunteeringSystem.Models
{
    public class Educator : Person
    {
        public Credentials credentials { get; set; }

        public string CFEP { get; set; }
    }
}
