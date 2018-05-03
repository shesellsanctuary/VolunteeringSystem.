using System.Collections.Generic;

namespace VolunteeringSystem.Models
{
    public class Kid
    {
        public PersonData personData { get; set; }

        public PartOfDay availability
        {
            get;
            set;
        }

        public HashSet<Activities> likes
        {
            get;
            set;
        }

        public HashSet<Activities> dislikes
        {
            get;
            set;
        }
    }
}
