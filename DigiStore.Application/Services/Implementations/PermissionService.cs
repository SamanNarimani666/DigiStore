using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.Permission;
using DigiStore.Domain.IRepositories.Role;
using DigiStore.Domain.IRepositories.RolePermission;
using DigiStore.Domain.IRepositories.User;
using DigiStore.Domain.IRepositories.UserRole;
using DigiStore.Domain.ViewModels;
using DigiStore.Domain.ViewModels.Roles;

namespace DigiStore.Application.Services.Implementations
{
    public class PermissionService: IPermissionService
    {
        #region Constructor
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUserRepository _userRepository;
        public PermissionService(IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IPermissionRepository permissionRepository, IRolePermissionRepository rolePermissionRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _userRepository = userRepository;
        }
        #endregion

        #region GetAllRole
        public async Task<List<Role>> GetAllRole()
        {
            return await _roleRepository.GetAllRole();
        }
        #endregion

        #region UserRolesId
        public List<int> UserRolesId(int userId)
        {
            return  _userRoleRepository.UserRolesId(userId).Result.Select(p => p.RoleId).ToList();
        }
        #endregion

        #region filterRoles
        public async Task<FilterRoleViewModel> filterRoles(FilterRoleViewModel filterRole)
        {
            return await _roleRepository.filterRoles(filterRole);
        }
        #endregion

        #region CreateRole
        public async Task<CreateRoleResult> CreateRole(CreateRoleViewModel createRole, List<int> SelectedPermission)
        {
            try
            {
                var newRole=new Role()
                {
                    RoleTitle = createRole.RoleTitle
                };
                await _roleRepository.AddRole(newRole);
                await _roleRepository.Save();
                await AddRolePermission(newRole.RoleId, SelectedPermission);
                await _rolePermissionRepository.Save();
                return CreateRoleResult.Success;
            }
            catch
            {
                return CreateRoleResult.Error;
            }
        }
        #endregion

        #region RoleInfoForEdit
        public async Task<EditRoleViewModel> RoleInfoForEdit(int roleId)
        {
            var role = await _roleRepository.GetRoleByRoleId(roleId);
            if (role == null) return null;
            return new EditRoleViewModel()
            {
                RoleTitle = role.RoleTitle,
                RoleId = roleId
            };
        }
        #endregion

        #region EditRole
        public async Task<EditRoleResult> EditRole(EditRoleViewModel editRole, List<int> SelectedPermission)
        {
            var role = await _roleRepository.GetRoleByRoleId(editRole.RoleId);
            if (role == null) return EditRoleResult.Error;
            role.RoleTitle = editRole.RoleTitle;
            try
            {
                _roleRepository.EditRole(role);
                await _roleRepository.Save();
                 _rolePermissionRepository.DeletePremissionsRole(editRole.RoleId);
                 await _rolePermissionRepository.Save();
                 await AddRolePermission(role.RoleId, SelectedPermission);
                 await _rolePermissionRepository.Save();
                return EditRoleResult.Success;
            }
            catch
            {
                return EditRoleResult.Error;
            }
        }
        #endregion

        #region AddRolePermission
        public async Task AddRolePermission(int roleId, List<int> permissionId)
        {
            try
            {
                foreach (var permissions in permissionId)
                {
                    await _rolePermissionRepository.AddRolePermission(new RolePermission()
                    {
                        RoleId = roleId,
                        PermissionId = permissions
                    });
                }
            }
            catch { }
        }
        #endregion

        #region GetAllPermission
        public async Task<List<Permission>> GetAllPermission()
        {
            return await _permissionRepository.GetAllPermission();
        }
        #endregion

        #region RoleSelectedPermission
        public List<int> RoleSelectedPermission(int roleId)
        {
            return _rolePermissionRepository.GetAllRolePermission(roleId).Result.Select(p => p.PermissionId)
                .ToList();
        }
        #endregion

        #region ChckePermission
        public bool ChckePermission(int permissionId, string userName)
        {
            var user =  _userRepository.GetUserByUserName(userName).Result;
            List<int> userRoles =  _userRoleRepository.UserRolesId(user.UserId).Result
                .Select(r => r.RoleId).ToList();
            if (!userRoles.Any()) return false;

            List<int> rolePermission = _rolePermissionRepository.GetRolePermissions(permissionId).Select(p => p.RoleId)
                .ToList();
            return rolePermission.Any(p => userRoles.Contains(p));
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _roleRepository.DisposeAsync();
            await _userRoleRepository.DisposeAsync();
            await _permissionRepository.DisposeAsync();
            await _rolePermissionRepository.DisposeAsync();
        }
        #endregion
    }
}
