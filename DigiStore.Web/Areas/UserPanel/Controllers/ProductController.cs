using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.ProductVisited;
using DigiStore.Web.PresentationExtensions;

namespace DigiStore.Web.Areas.UserPanel.Controllers
{
    public class ProductController : UserBaseController
    {
        #region Constructor
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region LastProductVisited
        [HttpGet("user-history")]
        public async Task<IActionResult> LastProductVisited(FilterProductVisitedViewModel filterProductVisited)
        {
            filterProductVisited.TakeEntity = 5;
            filterProductVisited.UserId = User.GetUserId();
            return View(await _productService.GetLastProductVisited(filterProductVisited));
        }
        #endregion
    }
}
