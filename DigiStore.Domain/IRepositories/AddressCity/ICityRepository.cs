using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;

namespace DigiStore.Domain.IRepositories.AddressCity
{
    public interface ICityRepository : IAsyncDisposable
    {
        Task<List<City>> GetListCityByStateId(int stateId);
     
    }
}
