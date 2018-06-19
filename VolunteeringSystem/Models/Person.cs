using System;
using System.ComponentModel.DataAnnotations;
using VolunteeringSystem.Domain;

namespace VolunteeringSystem.Models
{
    public class Person
    {
        private string _cpf;
        public int id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        public string name { get; set; }

        public DateTime birthDate { get; set; }

        public string CPF
        {
            get => _cpf;
            set
            {
                if (value != null)
                {
                    value = value.Trim().Replace(".", "").Replace("-", "");
                    if (!validate_cpf(value)) throw new ArgumentOutOfRangeException();
                    value = value.Insert(3, ".");
                    value = value.Insert(7, ".");
                    value = value.Insert(11, "-");
                }

                _cpf = value;
            }
        }

        public Sex sex { get; set; }

        public DateTime createdAt { get; set; }

        private static bool validate_cpf(string cpf)
        {
            if (cpf.Length != 11) return false;
            var helper = cpf.Substring(0, 9);
            var sum = 0;
            for (var i = 0; i < 9; i++) sum += int.Parse(helper[i].ToString()) * (10 - i);
            var remainder = sum % 11;
            if (remainder < 2) remainder = 0;
            else remainder = 11 - remainder;
            var digit = remainder.ToString();
            helper = cpf.Substring(0, 9) + digit;
            sum = 0;
            for (var i = 0; i < 10; i++) sum += int.Parse(helper[i].ToString()) * (11 - i);
            remainder = sum % 11;
            if (remainder < 2) remainder = 0;
            else remainder = 11 - remainder;
            return cpf.EndsWith(digit + remainder);
        }
    }
}