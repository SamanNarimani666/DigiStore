using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels;
using DigiStore.Domain.ViewModels.Roles;
using DigiStore.Web.Security;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Web.Areas.Admin.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class RoleController : AdminBaseController
    {
        #region Construtor
        private readonly IPermissionService _permissionService;
        public RoleController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        #endregion

        #region FilterRole
        [PermissionChecker(5)]
        public async Task<IActionResult> FilterRole(FilterRoleViewModel filterRole)
        {
            return View(await _permissionService.filterRoles(filterRole));
        }
        #endregion#

        #region CreateRole
        [PermissionChecker(6)]
        [HttpGet("create-role")]
        public async Task<IActionResult> CreateRole()
        {
            ViewBag.Permission = await _permissionService.GetAllPermission();
            return View();
        }
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRole, List<int> SelectedPermission)
        {
            if (ModelState.IsValid)
            {
                var res = await _permissionService.CreateRole(createRole, SelectedPermission);
                switch (res)
                {
                    case CreateRoleResult.Error:
                        TempData[ErrorMessage] = "خطا در ایجاد نقش جدید";
                        break;
                    case CreateRoleResult.Success:
                        TempData[SuccessMessage] = "نقش جدید با موفقیت ثبت شد";
                        return RedirectToAction("FilterRole", "Role");
                }
            }
            ViewBag.Permission = await _permissionService.GetAllPermission();
            return View(createRole);
        }
        #endregion

        #region Edit Role
        [PermissionChecker(7)]
        [HttpGet("edit-role/{roleId}")]
        public async Task<IActionResult> EditRole(int roleId)
        {
            var role = await _permissionService.RoleInfoForEdit(roleId);
            if (role == null) return NotFound();
            ViewBag.Permission = await _permissionService.GetAllPermission();
            ViewBag.SelectedPermissions = _permissionService.RoleSelectedPermission(roleId);
            return View(role);
        }
        [HttpPost("edit-role/{roleId}")]
        public async Task<IActionResult> EditRole(int roleId, EditRoleViewModel editRole, List<int> SelectedPermission)
        {
            if (ModelState.IsValid)
            {
                var res = await _permissionService.EditRole(editRole, SelectedPermission);
                switch (res)
                {
                    case EditRoleResult.Error:
                        TempData[ErrorMessage] = "خطا در نقش مورد نظر";
                        break;
                    case EditRoleResult.Success:
                        TempData[SuccessMessage] = "نقش مورد نظر با موفقیت ویرایش شد";
                        return RedirectToAction("FilterRole", "Role");
                }
            }
            var role = await _permissionService.RoleInfoForEdit(roleId);
            if (role == null) return NotFound();
            ViewBag.Permission = await _permissionService.GetAllPermission();
            ViewBag.SelectedPermissions = _permissionService.RoleSelectedPermission(roleId);
            return View(role);
        }

        #endregion
    }
}
