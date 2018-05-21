using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolunteeringSystem.Models;
using VolunteeringSystem.Domain;

namespace VolunteeringSystem.DAO
{
    public class VolunteerDAO
    {
        private string connString = "Server=volunteeringsystem-postgres-sp.cr0sahgirswg.sa-east-1.rds.amazonaws.com;Database=orphanage;Port=5432;User Id=vol_sys_postgres_db_admin;Password=valpwd4242;";

        public VolunteerDAO()
        {
        }

        public IEnumerable<Volunteer> GetAll()
        {
            using (var sql = new NpgsqlConnection(connString))
            {
                var list = sql.Query<Volunteer>("SELECT id, status, name, birthdate, cpf, sex, profession, address, phone, photo, criminalrecord FROM volunteer").AsList();
                return list;
            }
        }

        public IEnumerable<Volunteer> GetByStatus(int status)
        {
            using (var sql = new NpgsqlConnection(connString))
            {
                var list = sql.Query<Volunteer>("SELECT id, status, name, birthdate, cpf, sex, profession, address, phone, photo, criminalrecord, createdAt FROM volunteer WHERE status = @status ORDER BY createdAt", new { status = status }).AsList();
                return list;
            }
        }

        public Volunteer Get(int volunteerId)
        {
            using (var sql = new NpgsqlConnection(connString))
            {
                return sql.QueryFirstOrDefault<Volunteer>("SELECT id, status, name, birthdate, cpf, sex, profession, address, phone, photo, criminalrecord, createdAt FROM volunteer WHERE id = @id", new { id = volunteerId });
            }
        }

        public bool Add(Volunteer newVolunteer)
        {

            using (var sql = new NpgsqlConnection(connString))
            {
                try
                {
                    int setCredentialsResponse = sql.Execute(@"
                        INSERT INTO credentials (email, password)
                        VALUES  (@email, @password)",
                        new
                        {
                            email = newVolunteer.credentials.email,
                            password = newVolunteer.credentials.password
                        });
                } catch (Npgsql.PostgresException e)
                {
                    return false;
                }


                int response = sql.Execute(@"
                    INSERT INTO volunteer (name, birthdate, cpf, sex, status, profession, address, phone, photo, criminalRecord, credentials)
                    VALUES (@name, @birthdate, @cpf, @sex::SEX, @status, @profession, @address, @phone, @photo, @criminalRecord, @credentials)",
                    new
                    {
                        name = newVolunteer.name,
                        birthdate = newVolunteer.birthDate,
                        cpf = newVolunteer.CPF,
                        sex = char.ToLower(newVolunteer.sex.ToString().ElementAt(0)).ToString(),
                        status = VolunteerStatus.Waiting,
                        profession = newVolunteer.profession,
                        address = newVolunteer.address,
                        phone = newVolunteer.phone,
                        photo = newVolunteer.photo,
                        criminalRecord = newVolunteer.criminalRecord,
                        credentials = newVolunteer.credentials.email
                    });
                return Convert.ToBoolean(response);
            }
        }

        public bool ChangeStatus(int volunteerId, int newStatus)
        {
            using (var sql = new NpgsqlConnection(connString))
            {
                int response = sql.Execute(@"UPDATE volunteer SET status = @status WHERE id = @id", new { id = volunteerId, status = newStatus });
                return Convert.ToBoolean(response);
            }
        }

        public int Login(string email, string password)
        {
            using (var sql = new NpgsqlConnection(connString))
            {
                var volunteerId = sql.QueryFirstOrDefault<int>(@"SELECT id FROM volunteer WHERE credentials = @email", new { email = email });
                if (volunteerId > 0)
                {
                    var volunteerEmail = sql.QueryFirstOrDefault<string>(@"SELECT email FROM credentials WHERE email = @email AND password = @password", new { email = email, password = password });
                    if (!string.IsNullOrEmpty(volunteerEmail))
                        return volunteerId;
                }
            }

            return 0;
        }
    }
}
