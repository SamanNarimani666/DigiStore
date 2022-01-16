using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.RolePermission;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.RolePermission
{
    public class RolePermissionRepository: IRolePermissionRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public RolePermissionRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddRolePermission
        public async Task AddRolePermission(Domain.Entities.RolePermission rolePermission)
        {
            await _context.RolePermissions.AddAsync(rolePermission);
        }
        #endregion

        #region GetAllRolePermission
        public async Task<List<Domain.Entities.RolePermission>> GetAllRolePermission(int roleId)
        {
            return await _context.RolePermissions.Where(p => p.RoleId == roleId).ToListAsync();
        }
        #endregion

        #region DeletePremissionsRole
        public void DeletePremissionsRole(int roleId)
        {
            _context.RolePermissions.Where(r => r.RoleId == roleId).ToList().ForEach(r => _context.RolePermissions.Remove(r));

        }
        #endregion

        #region GetRolePermissions
        public List<Domain.Entities.RolePermission> GetRolePermissions(int permissionId)
        {
            return _context.RolePermissions.Where(p => p.PermissionId == permissionId).ToList();
        }
        #endregion

        #region Save
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Dispose
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
