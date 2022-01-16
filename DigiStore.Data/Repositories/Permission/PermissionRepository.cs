using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.Permission;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.Permission
{
    public class PermissionRepository : IPermissionRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public PermissionRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllPermission
        public async Task<List<Domain.Entities.Permission>> GetAllPermission()
        {
            return await _context.Permissions.ToListAsync();
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
        #endregion
    }
}
