using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.FavoriteProductUser;
using DigiStore.Domain.ViewModels.ProductVisited;
using DigiStore.Web.Http;
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

        #region User Favorit product
        [HttpGet("favorit-product-user")]
        public async Task<IActionResult> GetProductFavoritUser(FilterFavoritViewModel filterFavorit)
        {
            filterFavorit.TakeEntity = 10;
            filterFavorit.UserId = User.GetUserId();
            return View(await _productService.GetFavoriteProductUserByUserId(filterFavorit));
        }
        #endregion

        #region Delete User Favorit Product

        [HttpGet("delete-product-favorit/{favortiId}/{productId}")]
        public async Task<IActionResult> DeleteUserFavoritProduct(int favortiId,int productId)
        {
            var res = await _productService.DeleteFavoritProduct(favortiId,productId, User.GetUserId());
            if (res)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "محصول مورد نظر با موفقیت حذف شد",
                    null);
            }
            TempData[ErrorMessage] = "خطا در حذف محصول مورد نظر";
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "",
                null);
        }

        #endregion
    }
}
