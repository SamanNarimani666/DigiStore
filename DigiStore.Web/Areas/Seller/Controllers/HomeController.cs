using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Web.PresentationExtensions;

namespace DigiStore.Web.Areas.Seller.Controllers
{
    public class HomeController : SellerBaseController
    {
        private readonly ISellerService _sellerService;

        public HomeController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _sellerService.GetSellerInfoByUserId(User.GetUserId()));
        }
    }
}
