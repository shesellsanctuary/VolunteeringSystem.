using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VolunteeringSystem.Models
{
    public class Administrator : Person
    {
        public Credentials credentials { get; set; }
    }
}
