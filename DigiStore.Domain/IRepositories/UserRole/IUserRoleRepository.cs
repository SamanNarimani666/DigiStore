using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.UserRole
{
    public interface IUserRoleRepository:IAsyncDisposable
    {
        Task AddRoleToUser(List<int> roleIds, int userId);
        Task<List<Entities.UserRole>> UserRolesId(int userId);
        void DeleteUserRoles(int userId);
        Task Save();
    }
}
