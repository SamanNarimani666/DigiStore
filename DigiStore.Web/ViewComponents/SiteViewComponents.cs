using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Web.ViewComponents
{
    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public SiteHeaderViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategory();
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
