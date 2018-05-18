using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolunteeringSystem.Models;

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
                try
                {
                    var list = sql.Query<Volunteer>("SELECT id, name, birthdate, cpf, sex, profession, address, phone, photo, criminalrecord FROM volunteer").AsList();
                    return list;
                }
                catch (Exception e)
                {
                    var ee = e;
                }
                return new List<Volunteer>();
            }
        }

        public Volunteer Get(int volunteerId)
        {
            using (var sql = new NpgsqlConnection(connString))
            {
                try
                {
                    return sql.QueryFirstOrDefault<Volunteer>("SELECT id, name, birthdate, cpf, sex, profession, address, phone, photo, criminalrecord FROM volunteer WHERE id = @id", new { id = volunteerId });
                }
                catch (Exception e)
                {
                    var ee = e;
                }
                return new Volunteer();
            }
        }

        public bool Add(Volunteer newVolunteer)
        {
            using (var sql = new NpgsqlConnection(connString))
            {
                int response = sql.Execute(@"
                    INSERT INTO volunteer (name, birthdate, cpf, sex, profession, address, phone, photo, criminalRecord, credentials)
                    VALUES (@name, @birthdate, @cpf, @sex, @profession, @address, @phone, @photo, @criminalRecord, @credentials)",
                    new
                    {
                        name = newVolunteer.name,
                        birthdate = newVolunteer.birthDate,
                        cpf = newVolunteer.CPF,
                        sex = newVolunteer.sex,
                        profession = newVolunteer.profession,
                        address = newVolunteer.address,
                        phone = newVolunteer.phone,
                        photo = newVolunteer.photo,
                        criminalRecord = newVolunteer.criminalRecord,
                        credentials = newVolunteer.credentials
                    });

                return Convert.ToBoolean(response);
            }
        }

    }
}
