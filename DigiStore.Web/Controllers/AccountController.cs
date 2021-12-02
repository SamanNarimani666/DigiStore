using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DigiMarket.Web.HttpContext;
using DigiStore.Application.Senders;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Core.Convertors;
using DigiStore.Domain.ViewModels.Account;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;


namespace DigiStore.Web.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region Field
        private readonly IUserService _userService;
        private readonly ISender _sender;
        private readonly IViewRenderService _viewRenderService;
        private readonly ICaptchaValidator _captchaValidator;
        #endregion

        #region Constructor
        public AccountController(IUserService userService, ISender sender, IViewRenderService viewRenderService, ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _sender = sender;
            _viewRenderService = viewRenderService;
            _captchaValidator = captchaValidator;
        }
        #endregion

        #region Register
        [RedirectIfLoggedInActionFilter]
        [HttpGet("Register")]
        public  IActionResult Register()
        {
            return View();
        }
        [HttpPost("Register"),ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUser)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(registerUser.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
            }
            if (ModelState.IsValid)
            {
                var res = await _userService.RegisterUser(registerUser);
                switch (res)
                {
                    case RegisterResult.ExistMobile:
                        TempData[WarningMessage] = "شماره موبایل وارد شده قبلا استفاده شده است";
                        break;
                    case RegisterResult.ExistUserName:
                        TempData[WarningMessage] = "نام کاربری متعلق به کاربر دیگری می باشد";
                        break;
                    case RegisterResult.ExistsEmail:
                        TempData[WarningMessage] = "ایمیل وارد شده قبلا استفاده شده است";
                        break;
                    case RegisterResult.Failed:
                        TempData[ErrorMessage] = "خطا در ثبت نام";
                        break;
                    case RegisterResult.Success:
                        TempData[SuccessMessage] = "ثبت نام با موفقیت انجام شد جهت فعال سازی حساب کاربری خود ایمیل های خود را برسی کنید";
                        var user = await _userService.GetUserByEmail(registerUser.Email);
                        var body = _viewRenderService.RenderToStringAsync("Emails/ActivateEmail", user);
                        _sender.SendEmail(registerUser.Email, "فعال سازی حساب کاربری", body);
                        return RedirectToAction("Index", "Home");

                }
            }
            return View(registerUser);
        }

        #endregion

        #region ActiveAccount
        [HttpGet("activate-account/{activecode}")]
        public async Task<IActionResult> ActiveAccount(string activecode)
        {
            ViewBag.IsActive = await _userService.ActiveUserByActiveCode(activecode);
            return View();
        }
        #endregion

        #region Login
        [RedirectIfLoggedInActionFilter]
        [HttpGet("Login")]
        public  IActionResult Login(string ReturnUrl)
        {
            return View();
        }
        [HttpPost("Login"),ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login, string ReturnUrl)
        {
            if(!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
            }
            if (ModelState.IsValid)
            {
                var res = await _userService.LoginUser(login);
                switch (res)
                {
                    case LoginResult.NotActive:
                        TempData[WarningMessage] = "حساب کاربری شما فعال نشده است";
                        break;
                    case LoginResult.NotFound:
                        TempData[WarningMessage] = "حساب کاربری با این مشخصات یافت نشد";
                        break;
                    case LoginResult.UserIsBlock:
                        TempData[WarningMessage] = "حساب کاربری شما بلاک شده است";
                        break;
                    case LoginResult.Success:
                        TempData[SuccessMessage] = "ورود به دیجی استور موفقیت انجام شد";
                        var user = await _userService.GetUserByEmailOrPhone(login.EmailOrMobiel);
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name,user.UserName),
                            new Claim(ClaimTypes.MobilePhone,user.Mobile)
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe
                        };
                        await HttpContext.SignInAsync(principal, properties);
                        if (!string.IsNullOrEmpty(ReturnUrl))
                        {
                            TempData["data"] = "this is data from login";

                            return Redirect(ReturnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                }
            }
            return View(login);
        }
        #endregion

        #region LogOut
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
        #endregion
    }
}
