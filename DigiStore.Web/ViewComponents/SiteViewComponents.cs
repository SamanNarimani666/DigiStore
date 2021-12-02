using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Web.ViewComponents
{
    public class SiteHeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteHeader");
        }
    }

    public class SiteFooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteFooter");
        }
    }

    public class HomeSliderViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("HomeSlider");
        }
    }

    public class LoginInformationUserViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public LoginInformationUserViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("LoginInformationUser",await _userService.GetInformationUserForSidebarById(User.GetUserId()));
        }
    }
}
