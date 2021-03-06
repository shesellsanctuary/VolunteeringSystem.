﻿using System.Collections.Generic;
using VolunteeringSystem.Domain;

namespace VolunteeringSystem.Models
{
    public class Volunteer : Person
    {
        public VolunteerStatus status { get; set; }

        public string profession { get; set; }

        public string address { get; set; }

        public string phone { get; set; }

        public string photo { get; set; }

        public string criminalRecord { get; set; }

        public List<Evaluation> evaluations { get; set; }

        public Credentials credentials { get; set; }

        public int GetScore()
        {
            return 0;
        }
    }
}