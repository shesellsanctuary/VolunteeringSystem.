using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolunteeringSystem.Domain;

namespace VolunteeringSystem.Models
{
    public class Volunteer : Person
    {
        public String profession { get; set; }

        public String address { get; set; }

        public String phone { get; set; }

        public Uri photo { get; set; }

        public Uri criminalRecord { get; set; }

        public List<Evaluation> evaluations { get; set; }

        public Credentials credentials { get; set; }


        public int GetScore() {
            return 0;
        }
    }
}
