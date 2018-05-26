using System.Collections.Generic;
using Dapper;
using Npgsql;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.DAO
{
    public class KidDao
    {
        public bool Add(Kid kid)
        {
            return false;
        }

        public IEnumerable<Kid> GetAll()
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                return sql.Query<Kid>("SELECT * FROM kid").AsList();
            }
        }
    }
}