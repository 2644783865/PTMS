using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.IRepositories
{
    public interface IFactOfObjectRouteRepository : IDataServiceAsync<FactOfObjectRoute>
    {
        Task<List<FactOfObjectRoute>> GetByDateAsync(DateTime date);
    }
}
