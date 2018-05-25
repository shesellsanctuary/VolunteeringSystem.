using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.DAO
{
    public class VolunteerDAO
    {
        public IEnumerable<Volunteer> GetAll()
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.ConnectionString))
            {
                var list = sql
                    .Query<Volunteer>(
                        "SELECT id, status, name, birthdate, cpf, sex, profession, address, phone, photo, criminalrecord FROM volunteer")
                    .AsList();
                return list;
            }
        }

        public IEnumerable<Volunteer> GetByStatus(int status)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.ConnectionString))
            {
                var list = sql
                    .Query<Volunteer>(
                        "SELECT id, status, name, birthdate, cpf, sex, profession, address, phone, photo, criminalrecord, createdAt FROM volunteer WHERE status = @status ORDER BY createdAt",
                        new {status}).AsList();
                return list;
            }
        }

        public Volunteer Get(int volunteerId)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.ConnectionString))
            {
                return sql.QueryFirstOrDefault<Volunteer>(
                    "SELECT id, status, name, birthdate, cpf, sex, profession, address, phone, photo, criminalrecord, createdAt FROM volunteer WHERE id = @id",
                    new {id = volunteerId});
            }
        }

        public bool Add(Volunteer newVolunteer)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.ConnectionString))
            {
                try
                {
                    var setCredentialsResponse = sql.Execute(@"
                        INSERT INTO credentials (email, password)
                        VALUES  (@email, @password)",
                        new
                        {
                            newVolunteer.credentials.email,
                            newVolunteer.credentials.password
                        });
                }
                catch (PostgresException e)
                {
                    return false;
                }

                Console.WriteLine(newVolunteer.sex);

                var response = sql.Execute(@"
                    INSERT INTO volunteer (name, birthdate, cpf, sex, status, profession, address, phone, photo, criminalRecord, credentials)
                    VALUES (@name, @birthdate, @cpf, @sex::SEX, @status, @profession, @address, @phone, @photo, @criminalRecord, @credentials)",
                    new
                    {
                        newVolunteer.name,
                        birthdate = newVolunteer.birthDate,
                        cpf = newVolunteer.CPF,
                        sex = char.ToLower(newVolunteer.sex.ToString().ElementAt(0)).ToString(),
                        status = VolunteerStatus.Waiting,
                        newVolunteer.profession,
                        newVolunteer.address,
                        newVolunteer.phone,
                        newVolunteer.photo,
                        newVolunteer.criminalRecord,
                        credentials = newVolunteer.credentials.email
                    });
                return Convert.ToBoolean(response);
            }
        }

        public bool ChangeStatus(int volunteerId, int newStatus)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.ConnectionString))
            {
                var response = sql.Execute(@"UPDATE volunteer SET status = @status WHERE id = @id",
                    new {id = volunteerId, status = newStatus});
                return Convert.ToBoolean(response);
            }
        }

        public int Login(string email, string password)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.ConnectionString))
            {
                var volunteerId = sql.QueryFirstOrDefault<int>(@"SELECT id FROM volunteer WHERE credentials = @email",
                    new {email});
                if (volunteerId <= 0) return 0;
                var volunteerEmail = sql.QueryFirstOrDefault<string>(
                    @"SELECT email FROM credentials WHERE email = @email AND password = @password",
                    new {email, password});
                if (!string.IsNullOrEmpty(volunteerEmail))
                    return volunteerId;
            }

            return 0;
        }
    }
}