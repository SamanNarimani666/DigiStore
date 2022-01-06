using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.FavoriteProductUser;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Web.Http;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HttpExtensions = DigiStore.Web.HttpContext.HttpExtensions;

namespace DigiStore.Web.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region Constructor
        private readonly IProductService _productService;
        private readonly IBranadService _branadService;
        public ProductController(IProductService productService, IBranadService branadService)
        {
            _productService = productService;
            _branadService = branadService;
        }
        #endregion

        #region ProductSearch
        [HttpGet("ProductSearch")]
        [HttpGet("ProductSearch/{Category}")]
        public async Task<IActionResult> ProductSearch(FilterProductViewModel filterProduct)
        {

            filterProduct.TakeEntity = 12;
            filterProduct.FilterProductState = FilterProductState.Accepted;
            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategory();
            ViewBag.ProductBrands = await _branadService.GetAllBrands();
            filterProduct = await _productService.FilterProduct(filterProduct);

            return View(filterProduct);
        }
        #endregion

        #region Show Product Detials
        [HttpGet("products/{productId}/{title}")]
        public async Task<IActionResult> ProductDetail(int productId, string title)
        {
            var product = await _productService.GetProductDetail(productId);
            if (product == null) return NotFound();
            ViewBag.IsExistThisProductInUserfavortitList =
                await _productService.IsExistThisProductInUserFavoritList(productId, User.GetUserId());
            return View(product);
        }
        #endregion

        #region Visited
        [HttpGet("visited/{produtId}")]
        public async Task Visited(int produtId)
        {
            if (produtId != null && produtId != 0)
            {
                await _productService.AddProductVisited(produtId, HttpExtensions.GetUserIp(HttpContext),
                    IdentityExtensions.GetUserId(User));
            }
        }
        #endregion

        #region add to favorit Product
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddProductToFavorit(CreateIFavoriteProductUserViewModel createIFavoriteProductUser)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.CreateIFavoriteProductUser(createIFavoriteProductUser,
                    User.GetUserId());

                if (res)
                {
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success,
                        SuccessMessage = "محصول مورد نظر با موقیت ثبت شد", null);
                }
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                    SuccessMessage = "خطا در ثبت اطلاعات", null);
            }
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                SuccessMessage = "خطا در ثبت اطلاعات", null);
        }
        #endregion
    }
}
