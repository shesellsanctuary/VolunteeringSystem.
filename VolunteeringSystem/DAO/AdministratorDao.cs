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
        public Administrator Get(int administratorId)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var admin = sql.QueryFirstOrDefault<Administrator>("SELECT * FROM administrator WHERE id = @id", new {id = administratorId});
                admin.credentials = sql.QueryFirst<Credentials>("SELECT Email FROM administrator WHERE ID = @ID", new { admin.id });

                return admin;
            }
        }

        public Administrator Login(string email, string password)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var id = sql.QueryFirstOrDefault<int>(
                    @"SELECT id FROM administrator JOIN credential ON administrator.email = credential.email WHERE credential.email = @email AND credential.password = @password",
                    new {email, password});
                if (id == 0)
                    return null;
                return Get(id);
            }
        }

        public IEnumerable<Administrator> GetAll()
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var list = sql.Query<Administrator>(@"SELECT * FROM administrator").ToList();
                    list.ForEach(x => x.credentials = sql.QueryFirst<Credentials>(
                        "SELECT Email FROM administrator WHERE ID = @ID",
                        new { x.id }));
                return list;
            }
        }

        public bool Add(Administrator newAdministrator)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                try
                {
                    var setCredentialsResponse = sql.Execute(
                        @"INSERT INTO credential (email, password) VALUES (@email, @password)",
                        new
                        {
                            newAdministrator.credentials.email,
                            newAdministrator.credentials.password
                        });
                }
                catch (PostgresException)
                {
                    return false;
                }

                var response = sql.Execute(@"
                    INSERT INTO administrator (name, birthdate, cpf, sex, email)
                    VALUES (@name, @birthdate, @cpf, @sex::SEX, @email)",
                    new
                    {
                        newAdministrator.name,
                        birthdate = newAdministrator.birthDate,
                        cpf = newAdministrator.CPF,
                        sex = char.ToUpper(newAdministrator.sex.ToString().ElementAt(0)).ToString(),
                        email = newAdministrator.credentials.email
                    });
                return Convert.ToBoolean(response);
            }
        }
    }
}