using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Enums.Payment;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
namespace DigiStore.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region Constructor

        private readonly IProductService _productService;
        private readonly IProductDiscountService _discountService;
        public HomeController(IProductService productService, IProductDiscountService discountService)
        {
            _productService = productService;
            _discountService = discountService;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            ViewBag.OffProduct = await _discountService.GetAlloffProducs(8);
            ViewBag.PopularProducs = await _productService.GetPopularProduct(8);
            ViewBag.MostPopularProduct = await _productService.GetMostPopular(8);
            ViewBag.MostVisitedProduct = await _productService.TheMostVisitedProducts(8);
            if (User.Identity.IsAuthenticated) ViewBag.RecommendedproductsForUser=await _productService.RecommendedproductsForUser(8, User.GetUserId());
            return View();
        }

        public IActionResult Error404()=> View();
       
    }
}
