using System;
using System.Collections.Generic;
using Dapper;
using Npgsql;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.DAO
{
    /*
        - O pacote Npgsql é para criar/manipular a conexão com o BD Postgres.
        - O pacote Dapper é para poder fazer os SELECTS, INSERTS, DELETES no BD.
        Por exemplo, a linha "var list = sql.Query<Event>("SELECT * FROM event").AsList();"
        coloca os registros que vieram do "SELECT * FROM event" em uma lisa de objetos Event.
    */

    public class EventDao
    {
        public IEnumerable<Event> GetAll()
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var list = sql
                    .Query<Event>(
                        "SELECT id, institute, kid_limit, date, description, age_group_id, creation_date FROM event")
                    .AsList();
                return list;
            }
        }

        public IEnumerable<Event> GetByStatus(int status)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var list = sql.Query<Event>(
                    "SELECT status, id, institute, age_group_id as ageGroupId, kid_limit, date, description, creation_date as creationDate, volunteer_id as volunteerId, justification FROM event WHERE status = @status ORDER BY creation_date",
                    new {status}).AsList();
                return list;
            }
        }

        public Event Get(int eventId)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                return sql.QueryFirst<Event>(
                    "SELECT status, id, institute, age_group_id as ageGroupId, kid_limit, date, description, creation_date as creationDate, volunteer_id as volunteerId, justification FROM event WHERE id = @id",
                    new {id = eventId});
            }
        }

        /// <summary>
        ///     Save a new event
        /// </summary>
        /// <param name="Event"> event object </param>
        /// <returns> true: saved | false: error </returns>
        public bool Add(Event newEvent)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var response = sql.Execute(@"
                    INSERT INTO event (institute, age_group_id, kid_limit, date, description, volunteer_id)
                    VALUES (@institute, @ageGroup, @kidLimit, @date, @description, @volunteerId)",
                    new
                    {
                        newEvent.institute,
                        ageGroup = newEvent.ageGroup.id,
                        newEvent.kidLimit,
                        newEvent.date,
                        newEvent.description,
                        newEvent.volunteerId
                    });

                return Convert.ToBoolean(response);
            }
        }

        /// <summary>
        ///     Edit an event
        /// </summary>
        /// <param name="Event"> objeto Event </param>
        /// <returns> true: edited | false: error </returns>
        public bool Edit(Event editedEvent)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var response = sql.Execute(@"UPDATE event SET 
                                                institute = @institute,
                                                age_group_id = @ageGroup,
                                                kid_limit = @kid_limit,
                                                date = @date,
                                                description = @description
                                            WHERE ID=@id",
                    new
                    {
                        editedEvent.id,
                        editedEvent.institute,
                        editedEvent.ageGroup,
                        editedEvent.kidLimit,
                        editedEvent.date,
                        editedEvent.description
                    });
                return Convert.ToBoolean(response);
            }
        }

        /// <summary>
        ///     Remove the event
        /// </summary>
        /// <param name="ID"> event Id </param>
        public bool Remove(int id)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var response = sql.Execute("DELETE FROM event WHERE ID = @id", new {id});
                return Convert.ToBoolean(response);
            }
        }

        /// <summary>
        ///     Aprove or block an event
        /// </summary>
        /// <param name="id"> event object </param>
        /// <param name="status"> event status </param>
        /// <param name="justification"> event justification </param>
        /// <returns> true: edited | false: error </returns>
        public bool Homolog(int id, int status, string justification)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                var response = sql.Execute(@"UPDATE event SET 
                                                    status = @status, 
                                                    justification = @justification 
                                            WHERE id = @id",
                    new
                    {
                        id,
                        status,
                        justification
                    });

                return Convert.ToBoolean(response);
            }
        }

        public int Quantity(int? status)
        {
            using (var sql = new NpgsqlConnection(ConnectionProvider.GetConnectionString()))
            {
                if (status == null)
                    return sql.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM event");
                else
                    return sql.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM event WHERE status = " + status);
            }
        }
    }
}