using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;
using VolunteeringSystem.Domain;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.DAO
{
    public class VolunteerDao
    {
        public IEnumerable<Volunteer> GetAll()
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var list = sql
                    .Query<Volunteer>(
                        "SELECT id, status, name, birthdate, cpf, sex, profession, address, phone, photo, criminal_record FROM volunteer")
                    .AsList();
                return list;
            }
        }

        public IEnumerable<Volunteer> GetByStatus(int status)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var list = sql
                    .Query<Volunteer>(
                        "SELECT id, status, name, birthdate, cpf, sex, profession, address, phone, photo, criminal_record, creation_date FROM volunteer WHERE status = @status ORDER BY creation_date",
                        new {status}).AsList();
                return list;
            }
        }

        public Volunteer Get(int volunteerId)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                return sql.QueryFirstOrDefault<Volunteer>(
                    "SELECT id, status, name, birthdate, cpf, sex, profession, address, phone, photo, criminal_record, creation_date FROM volunteer WHERE id = @id",
                    new {id = volunteerId});
            }
        }

        public bool Add(Volunteer newVolunteer)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                try
                {
                    var setCredentialsResponse = sql.Execute(
                        @"INSERT INTO credential (email, password) VALUES (@email, @password)",
                        new
                        {
                            newVolunteer.credentials.email,
                            newVolunteer.credentials.password
                        });
                }
                catch (PostgresException)
                {
                    return false;
                }

                var response = sql.Execute(@"
                    INSERT INTO volunteer (name, birthdate, cpf, sex, status, profession, address, phone, photo, criminal_record, credentials)
                    VALUES (@name, @birthdate, @cpf, @sex::SEX, @status, @profession, @address, @phone, @photo, @criminal_record, @credentials)",
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
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var response = sql.Execute(@"UPDATE volunteer SET status = @status WHERE id = @id",
                    new {id = volunteerId, status = newStatus});
                return Convert.ToBoolean(response);
            }
        }

        public Volunteer Login(string email, string password)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                try
                {
                    var volunteerId = sql.QueryFirst<int>(
                        @"SELECT id FROM volunteer JOIN credential ON volunteer.email = credential.email WHERE credential.email = @email AND credential.password = @password",
                        new {email, password});
                    return Get(volunteerId);
                }
                catch (Exception)
                {
                    throw new ArgumentException("User not found in the database.");
                }
            }
        }
    }
}