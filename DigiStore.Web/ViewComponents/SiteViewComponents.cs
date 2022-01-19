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
        private readonly ISiteService _siteService;

        public SiteFooterViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteFooter",await _siteService.GetDefaultSiteSetting());
        }
    }

    public class HomeSliderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        public HomeSliderViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("HomeSlider",await _siteService.GetAllActiveSlider());
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
            return View("LoginInformationUser", await _userService.GetInformationUserForSidebarById(User.GetUserId()));
        }
    }
    public class UserOrderViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;
        public UserOrderViewComponent(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var openOrder = await _orderService.GetUserOpenOrderDetials(User.GetUserId());
            return View("UserOrder", openOrder);
        }
    }
    public class UserOrderResponsiveViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;
        public UserOrderResponsiveViewComponent(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var openOrder = await _orderService.GetUserOpenOrderDetials(User.GetUserId());
            return View("UserOrderResponsive", openOrder);
        }
    }

    public class BrandViewComponent : ViewComponent
    {
        private readonly IBranadService _branadService;

        public BrandViewComponent(IBranadService branadService)
        {
            _branadService = branadService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Brand",await _branadService.GetAllBrands());
        }
    }
    public class ProductRatingViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductRatingViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var productRating = await _productService.GetProductRatingByProductId(productId);
            return View("ProductRating", productRating);
        }
    }

    public class ProductScroreViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public ProductScroreViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var productRating = await _productService.GetProductRatingByProductId(productId);
            return View("ProductScrore", productRating);
        }
    }


}
