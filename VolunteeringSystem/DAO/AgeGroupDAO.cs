using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.DAO
{
    public class AgeGroupDAO
    {
        private string connString = "Server=volunteeringsystem-postgres-sp.cr0sahgirswg.sa-east-1.rds.amazonaws.com;Database=orphanage;Port=5432;User Id=vol_sys_postgres_db_admin;Password=valpwd4242;";

        public AgeGroupDAO()
        {
        }

        public IEnumerable<AgeGroup> GetAll()
        {
            using (var sql = new NpgsqlConnection(connString))
            {
                var list = sql.Query<AgeGroup>("SELECT * FROM ageGroup").AsList();
                return list;
            }
        }

        public List<SelectListItem> ToSelectList(IEnumerable<AgeGroup> ageGroups)
        {
            var selectList = new List<SelectListItem>();
            foreach (var item in ageGroups)
            {
                selectList.Add(new SelectListItem { Text = item.label, Value = item.label });
            }
            return selectList;
        }
    }
}
