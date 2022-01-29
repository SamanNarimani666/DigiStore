using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Web.Areas.Seller.ViewComponents
{
    #region SellerSidebarViewComponent

    public class SellerSidebarViewComponent : ViewComponent
    {
        #region Constructor
        private readonly ISellerService _sellerService;
        public SellerSidebarViewComponent(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }
        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SellerSidebar", await _sellerService.GetSellerInfoByUserId(User.GetUserId()));
        }
    }

    #endregion
}
