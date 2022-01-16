using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels;

namespace DigiStore.Domain.IRepositories.Role
{
    public interface IRoleRepository:IAsyncDisposable
    {
        Task<List<Entities.Role>> GetAllRole();
        Task<FilterRoleViewModel> filterRoles(FilterRoleViewModel filterRole);
        Task AddRole(Entities.Role role);
        void EditRole(Entities.Role role);
        Task<Entities.Role> GetRoleByRoleId(int roleId);
        Task Save();
    }
}
