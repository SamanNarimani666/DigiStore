using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Web.PresentationExtensions;
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
        public async Task<IActionResult> ProductDetail(int productId,string title)
        {
            var product = await _productService.GetProductDetail(productId);
            if (product==null) return NotFound();
            return View(product);
        }
        #endregion
        [HttpGet("visited/{produtId}")]
        public async Task Visited(int produtId)
        {
            if (produtId!=null&& produtId!=0)
            {
                await _productService.AddProductVisited(produtId, HttpExtensions.GetUserIp(HttpContext),
                    IdentityExtensions.GetUserId(User));
            }
        }
    }
}
