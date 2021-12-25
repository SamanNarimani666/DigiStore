using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Web.Areas.UserPanel.ViewComponents
{
   public class UserSidebarViewComponent : ViewComponent
   {
       private readonly IUserService _userService;
       private readonly ISellerService _sellerService;

       public UserSidebarViewComponent(IUserService userService, ISellerService sellerService)
       {
           _userService = userService;
           _sellerService = sellerService;
       }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.hasUserAnyActiveSellerPanel = await _sellerService.HasUserActiveSellerPanel(User.GetUserId());
            return View("UserSidebar",await _userService.GetInformationUserForSidebarById(User.GetUserId()));
        }
    }

   public class UserInformationViewComponent : ViewComponent
   {
       private readonly IUserService _userService;

       public UserInformationViewComponent(IUserService userService)
       {
           _userService = userService;
       }
       public async Task<IViewComponentResult> InvokeAsync()
       {
           return View("UserInformation",await _userService.GetInformationUserById(User.GetUserId()));
       }
   }
}
