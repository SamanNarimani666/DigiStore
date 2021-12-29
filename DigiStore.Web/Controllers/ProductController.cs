using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Web.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region Constructor
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region FilterProduct

        public async Task<IActionResult> FilterProduct(FilterProductViewModel filterProduct)
        {
            filterProduct.TakeEntity = 12;
            filterProduct = await _productService.FilterProduct(filterProduct);
            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategory();
            if (filterProduct.PageId > filterProduct.GetLastPage()) return NotFound();
            return View(filterProduct);
        }

        #endregion
    }
}
