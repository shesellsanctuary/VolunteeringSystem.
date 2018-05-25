using System;
using System.ComponentModel.DataAnnotations;

namespace VolunteeringSystem.Models
{
    public class Person
    {
        public int id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        public string name { get; set; }

        public DateTime birthDate { get; set; }

        public string CPF { get; set; }

        public Sex sex { get; }

        public DateTime createdAt { get; set; }
    }
}