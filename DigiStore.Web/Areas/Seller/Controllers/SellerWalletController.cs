using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.SellerWallet;
using DigiStore.Web.PresentationExtensions;

namespace DigiStore.Web.Areas.Seller.Controllers
{
    public class SellerWalletController : SellerBaseController
    {
        #region Constructor
        private readonly ISellerWalletService _sellerWalletService;
        private readonly ISellerService _sellerService;
        public SellerWalletController(ISellerWalletService sellerWalletService, ISellerService sellerService)
        {
            _sellerWalletService = sellerWalletService;
            _sellerService = sellerService;
        }
        #endregion

        #region seller-wallet
        [HttpGet("seller-wallet")]
        public async Task<IActionResult> Index(FilterSellerWalletViewModel filterSellerWallet)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
            if (seller == null) return NotFound();
            filterSellerWallet.SellerId = seller.SellerId;
            return View(await _sellerWalletService.FilterSellerWallet(filterSellerWallet));
        }
        #endregion

    }
}
