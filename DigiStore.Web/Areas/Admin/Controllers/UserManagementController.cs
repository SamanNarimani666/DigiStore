﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.User;
using DigiStore.Web.Security;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Web.Areas.Admin.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class UserManagementController : AdminBaseController
    {
        #region Constructor
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        public UserManagementController(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }
        #endregion

        #region FilterUser
        [PermissionChecker(1)]
        public async Task<IActionResult> Index(FilterUserViewModel filterUser)
        {
            return View(await _userService.FilterUser(filterUser));
        }
        #endregion

        #region DeleteUser
        [HttpGet]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var res = await _userService.DeleteUser(userId);
            if (res)
            {
                TempData[SuccessMessage] = "کاربر مورد تظر با موفقیت حذف شد";
            }
            else
            {
                TempData[ErrorMessage] = "خطا در حذف کاربر";

            }
            return RedirectToAction("Index", "UserManagement");
        }
        #endregion

        #region RestoreteUser
        [HttpGet]
        public async Task<IActionResult> RestoreteUser(int userId)
        {
            var res = await _userService.RestoreUser(userId);
            if (res)
            {
                TempData[SuccessMessage] = "کاربر مورد تظر با موفقیت بازگردانده شد";
            }
            else
            {
            TempData[ErrorMessage] = "خطا در بازگرداندن کاربر";
            }
            return RedirectToAction("Index", "UserManagement");
        }


        #endregion

        #region Create User
        [PermissionChecker(2)]
        [HttpGet("create-user")]
        public async Task<IActionResult> CreateUser()
        {
            ViewBag.Role = await _permissionService.GetAllRole();
            return View();
        }
        [HttpPost("create-user"),ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel createUser,List<int> SelectedRoles, IFormFile userAvatar)
        {
            if (ModelState.IsValid)
            {
                var res = await _userService.CreateUser(createUser, SelectedRoles, userAvatar);
                switch (res)
                {
                    case CreateUserResult.Error:
                        TempData[ErrorMessage] = "خطا در ایجاد کاربر جدید";
                        break;
                    case CreateUserResult.ExistMobile:
                        TempData[ErrorMessage] = "کاربری با این شماره تماس موجود می باشد";
                        break;
                    case CreateUserResult.ExistUserName:
                        TempData[ErrorMessage] = "نام کاربری وارد شده تکراری می باشد";
                        break;
                    case CreateUserResult.ExistsEmail:
                        TempData[ErrorMessage] = "ایمیل وارد شده تکراری می باشد";
                        break;
                    case CreateUserResult.Success:
                        TempData[SuccessMessage] = "کاربر جدید با موفقیت ثبت شد";
                        return RedirectToAction("Index", "UserManagement");
                }
            }
            ViewBag.Role = await _permissionService.GetAllRole();
            return View(createUser);
        }


        #endregion

        #region EditUser
        [HttpGet("edit-user/{userId}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int userId)
        {
            ViewBag.Role = await _permissionService.GetAllRole();
            ViewBag.UserRoles = _permissionService.UserRolesId(userId);
            return View(await _userService.UserInfoForEdit(userId));
        }
        [HttpPost("edit-user/{userId}")]
        public async Task<IActionResult> EditUser(int userId, EditUserForAdminViewModel editUserInfo,List<int> selectedRoles,IFormFile userAvatar)
        {
            if (ModelState.IsValid)
            {
                var res = await _userService.EditUserForAdmin(editUserInfo,selectedRoles,userAvatar);
                switch (res)
                {
                    case EditUserResult.Error:
                        TempData[ErrorMessage] = "خطا در ویرایش کاربر";
                        break;
                    case EditUserResult.NotFound:
                        TempData[ErrorMessage] = "کاربری با این مشخصات یافت نشد";
                        break;
                    case EditUserResult.Success:
                        TempData[SuccessMessage] = "ویرایش کاربر با موفقیت ثبت شد";
                        return RedirectToAction("Index", "UserManagement");
                }
            }
            ViewBag.Role = await _permissionService.GetAllRole();
            ViewBag.UserRoles = _permissionService.UserRolesId(userId);
            return View(editUserInfo);
        }

        #endregion
    }
}
