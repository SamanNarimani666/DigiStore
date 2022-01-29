using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Web.Http;
using Microsoft.AspNetCore.Mvc;


namespace DigiStore.Web.Areas.Admin.Controllers
{
    [Route("Product")]
    [AutoValidateAntiforgeryToken]
    public class ProductController : AdminBaseController
    {
        #region Constructor
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region ProductList
        [HttpGet("ProductList")]
        public async Task<IActionResult> Index(FilterProductViewModel filterProduct)
        {
            filterProduct.SellerId = null;
            return View(await _productService.FilterProduct(filterProduct));
        }
        #endregion

        #region Accept Product
        public async Task<IActionResult> AcceptSellerProduct(int id)
        {
            var result = await _productService.AcceptSellerProduct(id);
            if (result)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "محصول مورد نظر با موفقیت تایید شد", null);

            }
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "محصول مورد نظر یافت نشد", null);
        }
        #endregion

        #region reject Seller Product
        [HttpGet("RejectSellerProduct/{id}")]
        public async Task<IActionResult> RejectSellerProduct(int id)
        {
            return PartialView(await _productService.GetPorductInfoForReject(id));
        }
        [HttpPost("RejectSellerProduct/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectSellerProduct(RejectProductViewModel rejectProduct)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.RejectSellerProduct(rejectProduct);
                if (result)
                {
                    TempData[SuccessMessage] = "محصول مورد نظر با موفقیت رد شد";
                    return RedirectToAction("Index");

                }
                    TempData[SuccessMessage] = "محصولی با این مشخصات";
                    return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        #endregion

        #region ProductDetial
        [HttpGet("product-detial/{productId}")]
        public async Task<IActionResult> ProductDetilas(int productId)
        {
            var product = await _productService.GetProductDetail(productId);
            if (product == null) return NoContent();
            return View(product);
        }
        #endregion


    }
}
