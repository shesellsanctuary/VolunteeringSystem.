using System;

namespace VolunteeringSystem.Models
{
    public enum EventStatus
    {
        Waiting = 0,
        Approved = 1,
        Rejected = 2,
        Finished = 3
    }

    public static class EventStatusHelpers
    {
        public static string ToPortugueseString(this EventStatus status)
        {
            switch (status)
            {
                case EventStatus.Waiting:
                    return "Pendente";
                case EventStatus.Approved:
                    return "Aprovado";
                case EventStatus.Rejected:
                    return "Rejeitado";
                case EventStatus.Finished:
                    return "Realizado";
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}