using System;

namespace VolunteeringSystem.Models
{
    public class Person
    {
        public int id { get; set; }

        public string name { get; set; }

        public DateTime birthDate { get; set; }

        public string CPF { get; set; }

        public Sex sex { get; }

        public DateTime createdAt { get; set; }
    }
}
