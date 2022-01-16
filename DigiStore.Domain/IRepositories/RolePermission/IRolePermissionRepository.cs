using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.RolePermission
{
    public interface IRolePermissionRepository:IAsyncDisposable
    {
        Task AddRolePermission(Entities.RolePermission rolePermission);
        Task<List<Entities.RolePermission>> GetAllRolePermission(int roleId);
        void DeletePremissionsRole(int roleId);
        List<Entities.RolePermission> GetRolePermissions(int permissionId);
        Task Save();
    }
}
