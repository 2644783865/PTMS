﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.BusinessLogic.Models;

namespace PTMS.BusinessLogic.IServices
{
    public interface IBusStationService
    {
        Task<List<BusStationModel>> GetAllAsync();

        Task<BusStationModel> AddAsync(BusStationModel model);

        Task<BusStationModel> UpdateAsync(BusStationModel model);

        Task DeleteByIdAsync(int id);
    }
}
