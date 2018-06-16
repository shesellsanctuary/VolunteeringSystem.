using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.DAO
{
    public class ProfessionalDao
    {
        public Professional Get(int professionalId)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var professional = sql.QueryFirstOrDefault<Professional>("SELECT * FROM professional WHERE id = @id", new { id = professionalId });
                professional.credentials = sql.QueryFirst<Credentials>("SELECT Email FROM professional WHERE ID = @ID", new { professional.id });
                return professional;
            }
        }

        public Professional Login(string email, string password)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                try
                {
                    var id = sql.QueryFirst<int>(
                        @"SELECT id FROM professional JOIN credential ON professional.email = credential.email WHERE credential.email = @email AND credential.password = @password",
                        new { email, password });
                    return Get(id);
                }
                catch (Exception)
                {
                    throw new ArgumentException("User not found in the database.");
                }
            }
        }

        public IEnumerable<Professional> GetAll()
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var list = sql.Query<Professional>(@"SELECT * FROM professional").ToList();
                list.ForEach(x => x.credentials = sql.QueryFirst<Credentials>(
                    "SELECT email FROM professional WHERE ID = @ID",
                    new { x.id }));
                return list;
            }
        }

        public bool Add(Professional newProfessional)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                try
                {
                    var setCredentialsResponse = sql.Execute(
                        @"INSERT INTO credential (email, password) VALUES (@email, @password)",
                        new
                        {
                            newProfessional.credentials.email,
                            newProfessional.credentials.password
                        });
                }
                catch (PostgresException)
                {
                    return false;
                }

                var response = sql.Execute(@"
                    INSERT INTO professional (name, birthdate, cpf, sex, email, type, professionalid)
                    VALUES (@name, @birthdate, @cpf, @sex::SEX, @email, @type, @professionalId)",
                    new
                    {
                        newProfessional.name,
                        birthdate = newProfessional.birthDate,
                        cpf = newProfessional.CPF,
                        sex = char.ToUpper(newProfessional.sex.ToString().ElementAt(0)).ToString(),
                        email = newProfessional.credentials.email,
                        type = newProfessional.type,
                        professionalId = newProfessional.professionalId
                    });
                return Convert.ToBoolean(response);
            }
        }
    }
}
