using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Web.Areas.Admin.ViewComponents
{
    #region AdminUserInformation
    public class AdminUserInformation : ViewComponent
    {
        private readonly IUserService _userService;

        public AdminUserInformation(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("AdminUserInformation",await _userService.GetInformationUserForSidebarById(User.GetUserId()));
        }
    }
    #endregion

    #region SidebarMenu
    public class SidebarMenu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SidebarMenu");
        }
    }
    #endregion
}
