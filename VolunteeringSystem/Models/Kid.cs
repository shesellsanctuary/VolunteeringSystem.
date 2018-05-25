using System.Collections.Generic;

namespace VolunteeringSystem.Models
{
    public class Kid : Person
    {
        public PartOfDay availability { get; set; }

        public HashSet<Activities> likes { get; set; }

        public HashSet<Activities> dislikes { get; set; }
    }
}