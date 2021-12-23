using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;


namespace DigiStore.Web.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        #region Constructor
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        [HttpGet("ProductList")]
        public async Task<IActionResult> Index(FilterProductViewModel filterProduct)
        {
            filterProduct.SellerId = null;
            return View(await _productService.FilterProduct(filterProduct));
        }
    }
}
