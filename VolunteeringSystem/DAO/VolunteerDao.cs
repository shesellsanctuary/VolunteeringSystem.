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
            var volunteers = new List<Volunteer>();
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                foreach (var id in sql.Query<int>("SELECT id FROM volunteer").AsList()) volunteers.Add(Get(id));
            }

            return volunteers;
        }

        public IEnumerable<Volunteer> GetByStatus(int status)
        {
            var volunteers = new List<Volunteer>();
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var ids = sql.Query<int>("SELECT id FROM volunteer WHERE status = @status", new {status}).AsList();
                foreach (var id in ids) volunteers.Add(Get(id));
            }

            return volunteers;
        }

        public Volunteer Get(int id)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var volunteer = sql.QueryFirstOrDefault<Volunteer>(
                    "SELECT id, status, name, birthdate, cpf, sex, profession, address, phone, photo, criminal_record, creation_date FROM volunteer WHERE id = @id",
                    new {id});
                volunteer.credentials = sql.QueryFirst<Credentials>(
                    "SELECT * FROM credential JOIN volunteer ON credential.email = volunteer.email WHERE id = @id",
                    new {id});
                volunteer.evaluations =
                    sql.Query<Evaluation>(
                            "SELECT grade, comment FROM event WHERE volunteer_id = @id AND grade IS NOT NULL", new {id})
                        .AsList();
                return volunteer;
            }
        }

        public bool Add(Volunteer volunteer)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                try
                {
                    var setCredentialsResponse = sql.Execute(
                        @"INSERT INTO credential (email, password) VALUES (@email, @password)",
                        new
                        {
                            volunteer.credentials.email,
                            volunteer.credentials.password
                        });
                }
                catch (PostgresException)
                {
                    return false;
                }

                var response = sql.Execute(@"
                    INSERT INTO volunteer (name, birthdate, cpf, sex, status, profession, address, phone, photo, criminal_record, email)
                    VALUES (@name, @birthdate, @cpf, @sex::SEX, @status, @profession, @address, @phone, @photo, @criminal_record, @email)",
                    new
                    {
                        volunteer.name,
                        birthdate = volunteer.birthDate,
                        cpf = volunteer.CPF,
                        sex = char.ToUpper(volunteer.sex.ToString().ElementAt(0)).ToString(),
                        status = VolunteerStatus.Waiting,
                        volunteer.profession,
                        volunteer.address,
                        volunteer.phone,
                        volunteer.photo,
                        criminal_record = volunteer.criminalRecord,
                        volunteer.credentials.email
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

        public int Quantity(int? status)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                return status == null
                    ? sql.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM volunteer")
                    : sql.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM volunteer WHERE status = " + status);
            }
        }
    }
}