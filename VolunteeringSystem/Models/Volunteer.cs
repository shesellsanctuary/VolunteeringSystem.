using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolunteeringSystem.Domain;

namespace VolunteeringSystem.Models
{
    public class Volunteer
    {
        public String name
        {
            get;
            set;
        }
        public String profession
        {
            get;
            set;
        }
        public DateTime birthday
        {
            get;
            set;
        }
        public String cpf
        {
            get;
            set;
        }
        public String address
        {
            get;
            set;
        }
        public String phone
        {
            get;
            set;
        }
        // TODO: antecedentes criminais.
        public List<Evaluation> evaluations
        {
            get;
            set;
        }
    }
}
