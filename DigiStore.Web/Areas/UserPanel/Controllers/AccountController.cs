using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Extensions;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Account;
using DigiStore.Web.PresentationExtensions;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Web.Areas.UserPanel.Controllers
{
    [Route("Account")]
    public class AccountController : UserBaseController
    {
        private readonly IUserService _userService;


        public AccountController(IUserService userService)
        {
            _userService = userService;
    
        }

        

        
        [HttpGet("EditProfile")]
        public async Task<IActionResult> EditProfile()
        {
            return View(await _userService.InfoUserForEditProfile(User.GetUserId()));
        }
        [HttpPost("EditProfile"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditUserProfileViewModel editUserProfile,IFormFile UserAvatar)
        {
            if (ModelState.IsValid)
            {
                var res = await _userService.EditUserProfile(editUserProfile, UserAvatar, User.GetUserId());
                switch (res)
                {
                    case EditUserProfileResult.NotFound:
                        TempData[ErrorMessage] = "حساب کاربری مورد نظر یافت نشد";
                        break;
                    case EditUserProfileResult.UserIsBlock:
                        TempData[WarningMessage] = "حساب کاربری شما بلاک شده است";
                        break;
                    case EditUserProfileResult.NotActive:
                        TempData[WarningMessage] = "حساب کاربری شما فعال نمی باشد";
                        break;
                    case EditUserProfileResult.Success:
                        TempData[SuccessMessage] = "حساب کاربری شما با موفقیت ویرایش شد";
                        return RedirectToAction("Index", "Home", new{ area ="UserPanel"});
                }
            }
            return View(editUserProfile);
        }

        #region ChangePassWord
        [HttpGet("ChangePassWord")]
        public IActionResult ChangePassWord()
        {
            return View();
        }
        [HttpPost("ChangePassWord")]
        public async Task<IActionResult> ChangePassWord(ChangePasswordViewModel changePassword)
        {
           
            if (ModelState.IsValid)
            {
                var res = await _userService.ChangePassWord(changePassword, User.GetUserId());
                switch (res)
                {
                    case ChangePasswordResult.NotFound:
                        TempData[ErrorMessage] = "کاربری یافت نشد";
                        break;
                    case ChangePasswordResult.UnCurrentPassword:
                        TempData[WarningMessage] = "کلمه عبور قبلی اشتباه است";
                        ModelState.AddModelError("CurrentPassword", "کلمه عبور قبلی اشتباه است");
                        break;
                    case ChangePasswordResult.Error:
                        TempData[ErrorMessage] = "لطفا از کلمه ی عبور جدیدی استفاده کنید";
                        ModelState.AddModelError("NewPassword", "لطفا از کلمه ی عبور جدیدی استفاده کنید");
                        break;
                    case ChangePasswordResult.Success:
                        TempData[SuccessMessage] = "کلمه عبور با موفقیت انجام شد";
                        await HttpContext.SignOutAsync();
                        return RedirectToAction("Login", "Account", new { area = "" });
                }
            }
            return View(changePassword);
        }

        #endregion

        #region ProfilePersonalInfo
        [HttpGet("ProfilePersonalInfo")]
        public async Task<IActionResult> ProfilePersonalInfo()
        {
            return View(await _userService.GetInformationUserById(User.GetUserId()));
        }
        

        #endregion
    }
}
