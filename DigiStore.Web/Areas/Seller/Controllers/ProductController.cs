using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Domain.ViewModels.Seller;
using DigiStore.Web.Http;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace DigiStore.Web.Areas.Seller.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class ProductController : SellerBaseController
    {
        #region Constructor
        private readonly IProductService _productService;
        private readonly ISellerService _sellerService;
        private readonly IBranadService _branadService;
        public ProductController(IProductService productService, ISellerService sellerService, IBranadService branadService)
        {
            _productService = productService;
            _sellerService = sellerService;
            _branadService = branadService;
        }
        #endregion

        #region ProductList
        [HttpGet("Products-list")]
        public async Task<IActionResult> Index(FilterProductViewModel filterProduct)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
            filterProduct.SellerId = seller.SellerId;
            filterProduct.FilterProductState = FilterProductState.Active;
            var result = await _productService.FilterProduct(filterProduct);
            return View(result);
        }
        #endregion

        #region Create Product
        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.MainCategory = await _productService.GetAllProductCategoriesByParentId(null);
           
            return View();
        }
        [HttpPost("create-product"),ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel createProduct,IFormFile ProductImage)
        {
            if (ModelState.IsValid)
            {
                //Todo Add Product
            }
            ViewBag.MainCategory = await _productService.GetAllProductCategoriesByParentId(null);
          
            return View(createProduct);
        }
        #endregion

      
    }
}
