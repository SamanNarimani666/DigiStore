using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Application.Extensions;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.FavoriteProductUser;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Domain.ViewModels.ProductComment;
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
            if (produtId != 0)
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

        #region ProductCommnet
        [HttpGet("add-product-commnet/{productId}")]
        public async Task<IActionResult> ProductCommnet(int productId)
        {
            var product = await _productService.GetProductByProductId(productId);
            if (product == null) return NotFound();
            ViewBag.Product = product;
            return View(new CreateProductCommnetViewModel() { ProductId = productId });
        }
        [HttpPost("add-product-commnet/{productId}")]
        public async Task<IActionResult> ProductCommnet(CreateProductCommnetViewModel createProductCommnet)
        {
            var product = await _productService.GetProductByProductId(createProductCommnet.ProductId);
            if (product == null) return NotFound();
            if (ModelState.IsValid)
            {
                var res = await _productService.CreateProductCommnet(createProductCommnet, User.GetUserId());
                switch (res)
                {
                    case CreateProductCommnetResult.Error:
                        TempData[ErrorMessage] = "خطا در ثبت اطلاعات";
                        break;
                    case CreateProductCommnetResult.NotFoundProduct:
                        TempData[ErrorMessage] = "محصولی با این مشخصات یافت نشد";
                        break;
                    case CreateProductCommnetResult.Success:
                        TempData[SuccessMessage] = "کاربر گرامی نظر شما با موفقیت ثبت شد.";
                        TempData[InfoMessage] = "از اینکه به محصولات سایت ما توجه می کنید بی نهایت سپاس گذاریم";
                        return RedirectToAction("ProductDetail",
                            new { @productId = createProductCommnet.ProductId, @title = product.Name.FixTextForUrl() });
                }
            }
            ViewBag.Product = product;
            return View(createProductCommnet);
        }
        #endregion

        #region FilterProdutComment
        [HttpGet("filterProductComment/{productId}")]
        public async Task<IActionResult> FilterProdutComment(int productId, FilterProductCommentViewModel filterProductComment)
        {
            filterProductComment.ProductId = productId;
            return View(await _productService.filterFilterProductComment(filterProductComment));
        }

        #endregion

        #region test
        [HttpGet("test")]
        public async Task<IActionResult> Test(FilterProductViewModel filterProduct)
        {
            filterProduct.SelectedPrductCategories= new List<int>(){1,2,3};
            filterProduct.Name = "G";

            var test = await _productService.FilterForSiteSearch(filterProduct);

            await Task.Delay(4000);
            return View();
        }


        #endregion
    }
}
