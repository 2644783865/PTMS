using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Options;
using PTMS.Common;
using PTMS.DataServices.IRepositories;
using PTMS.DataServices.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class FactOfObjectRouteRepository : IFactOfObjectRouteRepository
    {
        private readonly string _tableName = "FACT_OBJECTS_OF_ROUTES";
        private readonly string _dataConnectionString;

        public FactOfObjectRouteRepository(IOptions<AppSettings> appSettings)
        {
            _dataConnectionString = appSettings.Value.DataDatabaseConnection;
        }

        public async Task<List<FactOfObjectRoute>> GetByDateAsync(DateTime date)
        {
            var result = new List<FactOfObjectRoute>();

            var startDate = date.Date;
            var endDate = date.Date.AddDays(1).AddSeconds(-1);

            var sql = $@"
select *
from {_tableName}
Where COUNT_OBJECTS_ > 0 
AND DATE_TIME_OF_OUTPUT >= '{startDate.ToString("yyyy-MM-dd HH:mm:ss")}'
AND DATE_TIME_OF_OUTPUT <= '{endDate.ToString("yyyy-MM-dd HH:mm:ss")}'
";

            using (var connection = new FbConnection(_dataConnectionString))
            {
                connection.Open();
                using (var command = new FbCommand(sql, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var item = MapToModel(reader);
                                result.Add(item);
                            }
                        }

                        return result;
                        
                    }
                }
            }
        }

        public async Task<FactOfObjectRoute> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<FactOfObjectRoute> AddAsync(FactOfObjectRoute entity)
        {
            throw new NotImplementedException();
        }

        public async Task<FactOfObjectRoute> UpdateAsync(FactOfObjectRoute entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        private FactOfObjectRoute MapToModel(DbDataReader reader)
        {
            var result = new FactOfObjectRoute()
            {
                Id = Convert.ToInt32(reader["IDS"]),
                RouteId = Convert.ToInt32(reader["ROUTE_"]),
                ProjectId = Convert.ToInt32(reader["PROJECT_"]),
                DateTimeOfOutput = Convert.ToDateTime(reader["DATE_TIME_OF_OUTPUT"]),
                CountObjects = Convert.ToInt32(reader["COUNT_OBJECTS_"])
            };

            return result;
        }
    }
}
