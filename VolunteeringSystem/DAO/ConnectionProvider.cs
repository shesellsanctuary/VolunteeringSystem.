using System;

namespace VolunteeringSystem.DAO
{
    public static class ConnectionProvider
    {
        public static string GetConnectionString()
        {
            return Environment.UserName == "mg"
                ? "Server=localhost;Database=volunteering_system;User Id=postgres"
                : "Server=volunteeringsystem-postgres-sp.cr0sahgirswg.sa-east-1.rds.amazonaws.com;Database=orphanage;Port=5432;User Id=vol_sys_postgres_db_admin;Password=valpwd4242;";
        }
    }
}