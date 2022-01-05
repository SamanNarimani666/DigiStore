using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Discount;
using DigiStore.Domain.ViewModels.ProductDiscount;
using DigiStore.Web.PresentationExtensions;

namespace DigiStore.Web.Areas.Seller.Controllers
{
    public class DiscountController : SellerBaseController
    {
        #region Constructor
        private readonly IProductDiscountService _discountService;
        private readonly ISellerService _sellerService;
        private readonly IProductService _productService;
        public DiscountController(IProductDiscountService discountService, ISellerService sellerService, IProductService productService)
        {
            _discountService = discountService;
            _sellerService = sellerService;
            _productService = productService;
        }
        #endregion

        #region FilterDiscount
        [HttpGet("dicounts")]
        [HttpGet("dicounts/{productId}")]
        public async Task<IActionResult> FilterDiscount(FilterProductDiscountViewModel filterProductDiscount)
        {
            if (filterProductDiscount.ProductId == null && filterProductDiscount.ProductId == 0) return NoContent();
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
            filterProductDiscount.SellerId = seller.SellerId;
            return View(await _discountService.FilterProductDiscount(filterProductDiscount));
        }
        #endregion

        #region CreateProductDiscount
        [HttpGet("create-discount/{productId}")]
        public async Task<IActionResult> CreateProductDiscount(int productId)
        {
            var product = await _productService.GetProductBySellerOwnerId(productId, User.GetUserId());
            if (product == null) return NotFound();
            ViewBag.product = product;
            return View(new CreateProductDiscountViewModel(){ProductId = productId});
        }

        [HttpPost("create-discount/{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductDiscount(int productId, CreateProductDiscountViewModel createProductDiscount)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
                var res = await _discountService.CreateProductDiscoun(createProductDiscount, seller.SellerId);
                switch (res)
                {
                    case CreateProductDiscountResult.Error:
                        TempData[ErrorMessage] = "خطا در ثبت کد تخفیف";
                        break;
                    case CreateProductDiscountResult.NotFoundProduct:
                        TempData[ErrorMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case CreateProductDiscountResult.NotFoundForSeller:
                        TempData[ErrorMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case CreateProductDiscountResult.Success:
                        TempData[SuccessMessage] = "عملیات ثبت تخفیف با موفقیت انجام شد";
                        return RedirectToAction("FilterDiscount");
                }
            }
            var product = await _productService.GetProductBySellerOwnerId(productId, User.GetUserId());
            if (product == null) return NotFound();
            ViewBag.product = product;
            return View(createProductDiscount);
        }
        #endregion
    }
}
