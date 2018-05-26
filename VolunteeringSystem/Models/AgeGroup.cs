namespace VolunteeringSystem.Models
{
    public class AgeGroup
    {
        public int id { get; set; }

        public int min { get; set; }

        public int max { get; set; }

        public string label { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var rhs = (AgeGroup) obj;
            return min == rhs.min && max == rhs.max;
        }

        public override string ToString()
        {
            return label + " (" + min + ", " + max + ")";
        }
    }
}