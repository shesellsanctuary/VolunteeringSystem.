using System;

namespace VolunteeringSystem.Models
{
    public class Event
    {
        public EventStatus status { get; set; }

        public int id { get; set; }

        public string institute { get; set; }

        public int ageGroupId { get; set; }

        public AgeGroup ageGroup { get; set; }

        public int kidLimit { get; set; }

        public DateTime date { get; set; }

        public string description { get; set; }

        public DateTime creationDate { get; set; }

        public int volunteerId { get; set; }

        public Volunteer volunteer { get; set; }

        public string justification { get; set; }
    }
}