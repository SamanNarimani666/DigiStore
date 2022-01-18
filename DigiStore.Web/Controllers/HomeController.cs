using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Enums.Payment;
using DigiStore.Domain.ViewModels.Contacts;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
namespace DigiStore.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region Constructor
        private readonly IProductService _productService;
        private readonly IProductDiscountService _discountService;
        private readonly ISiteService _siteService;
        public HomeController(IProductService productService, IProductDiscountService discountService, ISiteService siteService)
        {
            _productService = productService;
            _discountService = discountService;
            _siteService = siteService;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            ViewBag.OffProduct = await _discountService.GetAlloffProducs(8);
            ViewBag.PopularProducs = await _productService.GetPopularProduct(8);
            ViewBag.MostPopularProduct = await _productService.GetMostPopular(8);
            ViewBag.MostVisitedProduct = await _productService.TheMostVisitedProducts(8);
            if (User.Identity.IsAuthenticated) ViewBag.RecommendedproductsForUser=await _productService.RecommendedproductsForUser(8, User.GetUserId());
            return View();
        }
        #endregion

        #region Error404
        public IActionResult Error404() => View();
        #endregion

        #region ContactUs
        [HttpGet("contact-us")]
        public IActionResult ContactUs()
        {
            return View();
        }
        [HttpPost("contact-us")]
        public async Task<IActionResult> ContactUs(CreateContactUsViewModel contactUs)
        {
            if (ModelState.IsValid)
            {
                var res = await _siteService.CreateContact(contactUs,HttpContext.GetUserIp());
                switch (res)
                {
                    case CreateContactResult.Error:
                        TempData[ErrorMessage] = "خطا در ثبت اطلاعات";
                        break;
                    case CreateContactResult.Success:
                        TempData[SuccessMessage] = "تماس شما با موفیت ثبت شد";
                        TempData[InfoMessage] = "با تشکر از شما";
                        return RedirectToAction("Index", "Home");
                }
            }
            return View(contactUs);
        }
        #endregion

        #region AboutUs
        [HttpGet("about-us")]
        public async Task<IActionResult> AboutUs()
        {
            return View(await _siteService.GetDefaultSiteSetting());
        }
        #endregion
    }
}
