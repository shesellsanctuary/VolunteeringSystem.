using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.DAO
{
    public class AdministratorDao
    {
        public bool Add(Administrator administrator)
        {
            return false;
        }

        public Administrator Get(int administratorId)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                return sql.QueryFirstOrDefault<Administrator>(
                    "SELECT id, name, birthdate, cpf, sex, email FROM administrator WHERE id = @id",
                    new {id = administratorId});
            }
        }

        public Administrator Login(string email, string password)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                try
                {
                    var id = sql.QueryFirst<int>(
                        @"SELECT id FROM administrator JOIN credential ON administrator.email = credential.email WHERE credential.email = @email AND credential.password = @password",
                        new {email, password});
                    return Get(id);
                }
                catch (Exception)
                {
                    throw new ArgumentException("User not found in the database.");
                }
            }
        }

        public IEnumerable<Administrator> GetAll()
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var list = sql.Query<Administrator>("SELECT * FROM administrator").ToList();
                list.ForEach(x => x.credentials = sql.QueryFirst<Credentials>(
                    "SELECT * FROM credential JOIN administrator ON credential.email = administrator.email WHERE id = @id",
                    new {x.id}));
                return list;
            }
        }
    }
}