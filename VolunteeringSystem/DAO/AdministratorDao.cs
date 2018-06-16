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

        public bool Add(Medic newMedic)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                try
                {
                    var setCredentialsResponse = sql.Execute(
                        @"INSERT INTO credential (email, password) VALUES (@email, @password)",
                        new
                        {
                            newMedic.credentials.email,
                            newMedic.credentials.password
                        });
                }
                catch (PostgresException)
                {
                    return false;
                }

                var response = sql.Execute(@"
                    INSERT INTO medic (name, birthdate, cpf, sex, email, crm)
                    VALUES (@name, @birthdate, @cpf, @sex::SEX, @email, @crm)",
                    new
                    {
                        newMedic.name,
                        birthdate = newMedic.birthDate,
                        cpf = newMedic.CPF,
                        sex = char.ToUpper(newMedic.sex.ToString().ElementAt(0)).ToString(),
                        email = newMedic.credentials.email,
                        crm = newMedic.CRM
                    });
                return Convert.ToBoolean(response);
            }
        }

        public bool Add(Psychology newPsychology)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                try
                {
                    var setCredentialsResponse = sql.Execute(
                        @"INSERT INTO credential (email, password) VALUES (@email, @password)",
                        new
                        {
                            newPsychology.credentials.email,
                            newPsychology.credentials.password
                        });
                }
                catch (PostgresException)
                {
                    return false;
                }

                var response = sql.Execute(@"
                    INSERT INTO psychology (name, birthdate, cpf, sex, email, cfp)
                    VALUES (@name, @birthdate, @cpf, @sex::SEX, @email, @cfp)",
                    new
                    {
                        newPsychology.name,
                        birthdate = newPsychology.birthDate,
                        cpf = newPsychology.CPF,
                        sex = char.ToUpper(newPsychology.sex.ToString().ElementAt(0)).ToString(),
                        email = newPsychology.credentials.email,
                        cfp = newPsychology.CFP,
                    });
                return Convert.ToBoolean(response);
            }
        }

        public bool Add(Educator newEducator)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                try
                {
                    var setCredentialsResponse = sql.Execute(
                        @"INSERT INTO credential (email, password) VALUES (@email, @password)",
                        new
                        {
                            newEducator.credentials.email,
                            newEducator.credentials.password
                        });
                }
                catch (PostgresException)
                {
                    return false;
                }

                var response = sql.Execute(@"
                    INSERT INTO educator (name, birthdate, cpf, sex, email, cfep)
                    VALUES (@name, @birthdate, @cpf, @sex::SEX, @email, @cfep)",
                    new
                    {
                        newEducator.name,
                        birthdate = newEducator.birthDate,
                        cpf = newEducator.CPF,
                        sex = char.ToUpper(newEducator.sex.ToString().ElementAt(0)).ToString(),
                        email = newEducator.credentials.email,
                        cfep = newEducator.CFEP
                    });
                return Convert.ToBoolean(response);
            }
        }
    }
}