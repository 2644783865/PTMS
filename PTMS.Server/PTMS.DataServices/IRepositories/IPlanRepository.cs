using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;

namespace PTMS.DataServices.IRepositories
{
    public interface IPlanRepository : IDataServiceAsync<Plans>
    {
        Task<List<Plans>> GetByDateAsync(DateTime date);
    }
}
