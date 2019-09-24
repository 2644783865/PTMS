using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Options;
using PTMS.Common;
using PTMS.DataServices.IRepositories;
using PTMS.DataServices.Models;
using PTMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class BusDataRepository : IBusDataRepository
    {
        private readonly string _dataConnectionString;

        public BusDataRepository(IOptions<AppSettings> appSettings)
        {
            _dataConnectionString = appSettings.Value.DataDatabaseConnection;
        }

        public async Task<List<TrolleybusTodayStatus>> GetTrolleybusTodayStatus()
        {
            var result = new List<TrolleybusTodayStatus>();
            var sql = "EXECUTE PROCEDURE TROLL_V3009";

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
                                var item = new TrolleybusTodayStatus()
                                {
                                    Name = Convert.ToString(reader["NAME"]),
                                    RouteName = Convert.ToString(reader["ROUTE_NAME"]),
                                    Place = Convert.ToString(reader["PLACE"]),
                                    CoordTime = reader["COORD_TIME"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["COORD_TIME"])
                                };

                                result.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task UpdateBusRoutes(Objects vehicle)
        {
            var sql = $@"
UPDATE busdata b 
SET b.rout_ = {vehicle.LastRout} 
WHERE b.obj_id_ = {vehicle.ObjId} 
    AND b.proj_id_ = {vehicle.ProjId} 
    AND b.time_ BETWEEN CURRENT_DATE AND CURRENT_DATE + 1 
    AND b.rout_ != {vehicle.LastRout}
";

            using (var connection = new FbConnection(_dataConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new FbCommand(sql, connection, transaction))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            transaction.Commit();
                        }
                    }
                }
            }
        }
    }
}
