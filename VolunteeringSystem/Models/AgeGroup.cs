using System;
namespace VolunteeringSystem.Models
{
    public class AgeGroup
    {
        public int id { get; set; }

        public int min { get; set; }

        public int max { get; set; }

        public string label { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            AgeGroup rhs = (AgeGroup) obj;
            return min == rhs.min && max == rhs.max;
        }
        
    }
}
