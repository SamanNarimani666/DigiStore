using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels;
using DigiStore.Domain.ViewModels.Roles;

namespace DigiStore.Application.Services.Interfaces
{
    public interface IPermissionService:IAsyncDisposable
    {
        Task<List<Domain.Entities.Role>> GetAllRole();
        List<int> UserRolesId(int userId);
        Task<FilterRoleViewModel> filterRoles(FilterRoleViewModel filterRole);
        Task<CreateRoleResult> CreateRole(CreateRoleViewModel createRole, List<int> SelectedPermission);
        Task<EditRoleViewModel> RoleInfoForEdit(int roleId);
        Task<EditRoleResult> EditRole(EditRoleViewModel editRol, List<int> SelectedPermission);
        Task AddRolePermission(int roleId,List<int>permissionId);
        Task<List<Domain.Entities.Permission>> GetAllPermission();
        List<int> RoleSelectedPermission(int roleId);
        bool ChckePermission(int permissionId, string userName);
    }
}
