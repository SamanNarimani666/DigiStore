using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.AddressCity;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.AddressCity
{
    public class CityRepository: ICityRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public CityRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region GetListCityByStateId
        public async Task<List<City>> GetListCityByStateId(int stateId)
        {
            return await _context.Cities.Where(p => p.StateId == stateId).ToListAsync();
        }
        #endregion

        #region DisposeAsync
        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }
        #endregion
    }
}
