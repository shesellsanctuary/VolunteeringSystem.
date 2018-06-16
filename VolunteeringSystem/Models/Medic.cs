namespace VolunteeringSystem.Models
{
    public class Medic : Person
    {
        public Credentials credentials { get; set; }

        public string CRM { get; set; }
    }
}
