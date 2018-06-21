using System.Collections.Generic;
using System.Linq;
using System;
using Dapper;
using Npgsql;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.DAO
{
    public class KidDao
    {
        public bool Add(Kid newKid)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var response = sql.Execute(@"
                    INSERT INTO kid (name, birthdate, sex, availability)
                    VALUES (@name, @birthdate, @sex::SEX, availability)",
                    new
                    {
                        newKid.name,
                        birthdate = newKid.birthDate,
                        sex = char.ToUpper(newKid.sex.ToString().ElementAt(0)).ToString(),
                        availability = newKid.availability
                    });
                return Convert.ToBoolean(response);
            }
        }

        public IEnumerable<Kid> GetAll()
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                return sql.Query<Kid>("SELECT * FROM kid").AsList();
            }
        }

        public int Quantity()
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                return sql.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM kid");
            }
        }

        public Kid Get(int kidId)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var kid = sql.QueryFirstOrDefault<Kid>("SELECT * FROM kid WHERE id = @id", new { id = kidId });
                return kid;
            }
        }
    }
}