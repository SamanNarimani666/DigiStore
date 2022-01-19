using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Web.ViewComponents
{
    #region SiteHeader
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
    #endregion

    #region SiteFooter
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
    #endregion

    #region HomeSlider
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
    #endregion

    #region LoginInformationUser
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
    #endregion

    #region UserOrder
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
    #endregion

    #region UserOrderResponsive
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
    #endregion

    #region BrandSlider
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
    #endregion

    #region ProductRating
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
    #endregion

    #region ProductScrore
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
    #endregion

    #region ProductSlider
    public class ProductSliderViewComponent : ViewComponent
    {
        #region Constructor
        private readonly IProductService _productService;
        public ProductSliderViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        #endregion
        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            ViewBag.category = await _productService.GetProductCategoryByCategoryId(categoryId);
            return View("ProductSlider",await _productService.GetAllActiveProductByCategoryId(categoryId,8));
        }
    }
    #endregion
}
