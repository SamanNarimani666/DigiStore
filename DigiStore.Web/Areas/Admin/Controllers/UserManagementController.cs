using Microsoft.AspNetCore.Mvc;
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
        [PermissionChecker(6)]
        public async Task<IActionResult> Index(FilterUserViewModel filterUser)
        {
            return View(await _userService.FilterUser(filterUser));
        }
        #endregion

        #region DeleteUser
        [PermissionChecker(10)]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(DeleteAndRestoreUserViewModel deleteAndRestoreUser)
        {
            var res = await _userService.DeleteUser(deleteAndRestoreUser.UserId);
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
        [PermissionChecker(11)]
        [HttpPost]
        public async Task<IActionResult> RestoreteUser(DeleteAndRestoreUserViewModel deleteAndRestoreUser)
        {
            var res = await _userService.RestoreUser(deleteAndRestoreUser.UserId);
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
        [PermissionChecker(8)]
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
        [PermissionChecker(9)]

        [HttpGet("edit-user/{userId}")]
        public async Task<IActionResult> EditUser(int userId)
        {
            ViewBag.Role = await _permissionService.GetAllRole();
            ViewBag.UserRoles = _permissionService.UserRolesId(userId);
            return View(await _userService.UserInfoForEdit(userId));
        }
        [HttpPost("edit-user/{userId}"),ValidateAntiForgeryToken]
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

        #region UserDetials
        [PermissionChecker(7)]
        [HttpGet("user-details/{userId}")]
        public async Task<IActionResult> UserDetials(int userId)
        {
            var user = await _userService.GetUserDetialByProductId(userId: userId);
            if (user == null) return NoContent();
            return View(user);
        }

        #endregion
    }
}
