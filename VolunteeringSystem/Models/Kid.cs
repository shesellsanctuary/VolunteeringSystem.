using System;
using System.Collections.Generic;

namespace VolunteeringSystem.Models
{
    public class Kid
    {
        public DateTime birthDate
        {
            get;
            set;
        }

        public Sex sex
        {
            get;
        }

        public PartOfDay availability
        {
            get;
            set;
        }

        public HashSet<Activities> preferences
        {
            get;
            set;
        }

        public Kid()
        {
        }


    }
}
