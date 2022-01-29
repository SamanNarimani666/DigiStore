using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.UserRole;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.UserRole
{
    public class UserRoleRepository : IUserRoleRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;

        public UserRoleRepository(DigiStore_DBContext context)
        {
            _context = context;
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

        #region AddRoleToUser
        public async Task AddRoleToUser(List<int> roleIds, int userId)
        {
            foreach (var roleId in roleIds)
            {
                await _context.UserRoles.AddAsync(new Domain.Entities.UserRole()
                {
                    UserId = userId,
                    RoleId = roleId
                });
            }
        }
        #endregion

        #region UserRolesId
        public async Task<List<Domain.Entities.UserRole>> UserRolesId(int userId)
        {
            return await _context.UserRoles.Include(p=>p.Role).Where(p=>p.UserId==userId&&!p.Role.IsDelete).ToListAsync();
        }
        #endregion

        #region DeleteUserRoles
        public void DeleteUserRoles(int userId)
        {
            _context.UserRoles.Where(r=>r.UserId==userId).ToList().ForEach(r=> _context.UserRoles.Remove(r));
        }
        #endregion

        #region Save
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
