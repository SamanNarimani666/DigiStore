using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Order;
using DigiStore.Web.PresentationExtensions;
namespace DigiStore.Web.Areas.UserPanel.Controllers
{
    public class HomeController : UserBaseController
    {
        #region Contructor
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        public HomeController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }
        #endregion

        #region  Dashboard
        [Route("")]
        public async Task<IActionResult> Dashboard(FilterOrderViewModel filterOrder)
        {
            ViewBag.RecommendedproductsForUser = await _productService.RecommendedproductsForUser(8, User.GetUserId());
            filterOrder.UserId = User.GetUserId();
            return View(await _orderService.FilterOrder(filterOrder));
        }
        #endregion
    }
}
