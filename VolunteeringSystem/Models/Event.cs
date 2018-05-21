using System;

namespace VolunteeringSystem.Models
{
    public class Event
    {
        public int id { get; set; }

        public string institute { get; set; }

        public int ageGroupId { get; set; }

        public AgeGroup ageGroup { get; set; }

        public int kidLimit { get; set; }

        public DateTime date { get; set; }

        public DateTime createdAt { get; set; }

        public string description { get; set; }

        public int volunteerId { get; set; }

        public Volunteer volunteer { get; set; }
    }
}
