using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.Role;
using DigiStore.Domain.ViewModels;
using DigiStore.Domain.ViewModels.Paging;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.Role
{
    public class RoleRepository : IRoleRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public RoleRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllRole
        public async Task<List<Domain.Entities.Role>> GetAllRole()
        {
            return await _context.Roles.Where(p => !p.IsDelete).ToListAsync();
        }
        #endregion

        #region filterRoles
        public async Task<FilterRoleViewModel> filterRoles(FilterRoleViewModel filterRole)
        {
            var query =  _context.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(filterRole.RoleTitle))
            {
                query = query.Where(p => EF.Functions.Like(p.RoleTitle,$"%{filterRole.RoleTitle}%"));
            }

            var pager = Pager.Build(filterRole.PageId, await query.CountAsync(), filterRole.TakeEntity,
                filterRole.HowManyShowPageAfterAndBefore);
            var allProduct = query.Paging(pager).ToList();
            return filterRole.SetPaging(pager).SetRoles(allProduct);
        }
        #endregion

        #region AddRole
        public async Task AddRole(Domain.Entities.Role role)
        {
            await _context.Roles.AddAsync(role);
        }
        #endregion

        #region EditRole
        public void EditRole(Domain.Entities.Role role)
        {
            role.ModifiedDate=DateTime.Now;;
            _context.Roles.Update(role);
        }
        #endregion

        #region GetRoleByRoleId
        public async Task<Domain.Entities.Role> GetRoleByRoleId(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(p => p.RoleId == roleId);
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
