using System;

namespace VolunteeringSystem.Domain
{
    public enum VolunteerStatus
    {
        Waiting = 0,
        Active = 1,
        Blocked = 2
    }

    public static class VolunteerStatusHelpers
    {
        public static string ToPortugueseString(this VolunteerStatus status)
        {
            switch (status)
            {
                case VolunteerStatus.Waiting:
                    return "Pendente";
                case VolunteerStatus.Active:
                    return "Aprovado";
                case VolunteerStatus.Blocked:
                    return "Rejeitado";
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}