using System;

namespace VolunteeringSystem.Models
{
    public class Event
    {
        public string institute { get; set; }

        public AgeGroup ageGroup { get; set; }

        public int kidLimit { get; set; }

        public DateTime date { get; set; }

        public string description { get; set; }
    }
}
