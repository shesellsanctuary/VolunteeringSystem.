namespace VolunteeringSystem.Models
{
    public class Professional : Person
    {
        public Credentials credentials { get; set; }

        public string type { get; set; }

        public string professionalId { get; set; }
    }
}
