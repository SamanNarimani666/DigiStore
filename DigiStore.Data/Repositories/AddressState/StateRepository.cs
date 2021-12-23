using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.AddressState;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.AddressState
{
    public class StateRepository: IStateRepository
    {
        #region Constructor

        private readonly DigiStore_DBContext _context;
        public StateRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllState
        public async Task<List<State>> GetAllState()
        {
            return await _context.States.ToListAsync();
        }
        #endregion

        #region GetStateById
        public async Task<State> GetStateById(int stateId)
        {
            return await _context.States.FindAsync(stateId);
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
