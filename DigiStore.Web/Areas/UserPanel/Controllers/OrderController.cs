using System.Threading.Tasks;
using DigiStore.Application.Extensions;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Enums.Payment;
using DigiStore.Domain.ViewModels.Order;
using DigiStore.Web.Http;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DigiStore.Web.Areas.UserPanel.Controllers
{
    public class OrderController : UserBaseController
    {
        #region Constructor
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IPaymentSerivce _paymentSerivce;
        private readonly IAddressService _addressService;
        public OrderController(IOrderService orderService, IUserService userService, IPaymentSerivce paymentSerivce, IAddressService addressService)
        {
            _orderService = orderService;
            _userService = userService;
            _paymentSerivce = paymentSerivce;
            _addressService = addressService;
        }
        #endregion

        #region AddProductToOrder
        [AllowAnonymous]
        [HttpPost("add-product-to-order")]
        public async Task<IActionResult> AddProductToOrder(AddProductToOrderViewModel addProductToOrder)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    await _orderService.AddProductToOpenOrder(User.GetUserId(), addProductToOrder);
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success,
                        SuccessMessage = "محصول مورد نظر با موقیت ثبت شد", null);
                }

                TempData[ErrorMessage] = "جهت ثبت محصول در سبد خرید ابتدا وارد سایت شوید";
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                        "جهت ثبت محصول در سبد خرید ابتدا وارد سایت شوید"
                        , null);
            }
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                "در ثبت اطلاعات خطایی رخ داده است"
                , null);
        }
        #endregion

        #region Open User Order
        [HttpGet("open-order")]
        public async Task<IActionResult> UserOpenOrder()
        {
            var openOrder = await _orderService.GetUserOpenOrderDetials(User.GetUserId());
            return View(openOrder);
        }
        #endregion

        #region pay order
        [HttpGet("pay-order")]
        public async Task<IActionResult> PayUserOrderPrice()
        {
            var openOrderAmount = await _orderService.GetTotalOrderPriceForPayment(User.GetUserId());
            string callbackUrl = PathExtension.SiteUrl + Url.RouteUrl("ZarinPalPaymentResult");

            string redirectUrl = "";
            var status = _paymentSerivce.CreatePaymentRequest(null, openOrderAmount, "پرداخت سبد خرید", callbackUrl, ref redirectUrl);
            if (status == PaymentStatus.St100)
            {
                return Redirect(redirectUrl);
            }
            return RedirectToAction("UserOpenOrder");
        }
        #endregion

        #region Call back zarinpal 
        [AllowAnonymous]
        [HttpGet("payment-result", Name = "ZarinPalPaymentResult")]
        public async Task<IActionResult> CallBackZarinPal()
        {
            ViewBag.PayStatus = false;
            string authority = _paymentSerivce.GetAuthorityCodeFromCallback(HttpContext);
            if (authority == "")
            {
                ViewBag.PayStatus = false;
                TempData[WarningMessage] = "عملیات پرداخت با شکست مواجه شد";
            }
            var openOrderAmount = await _orderService.GetTotalOrderPriceForPayment(User.GetUserId());
            long refId = 0;
            var res = _paymentSerivce.PaymentVerification(null, authority, openOrderAmount, ref refId);
            if (res == PaymentStatus.St100)
            {
                ViewBag.PayStatus = true;
                ViewBag.RefId = refId;
                TempData[SuccessMessage] = "پرداخت شما با موفقیت انجام شد";
                TempData[InfoMessage] = "کد پیگیری" + refId;
                await _orderService.PayOrderProductPriceToSeller(User.GetUserId(), refId);
                return View();
            }
            return View();
        }
        #endregion

        #region OpenOrderPartial
        [HttpGet("change-detialCount/{detialId}/{count}")]
        public async Task ChangedetialCount(int detialId, int count)
        {
            await _orderService.ChangeOrderQty(detialId, User.GetUserId(), count);
        }
        #endregion

        #region Remove Product From Order
        [HttpGet("remove-order-item/{detailId}")]
        public async Task<IActionResult> RemoveProductFromOrder(int detailId)
        {
            var res = await _orderService.RemoveOrderDetial(detailId, User.GetUserId());
            if (res)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "محصول مورد نظر با موفقیت حذف شد", null);

            }
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "محصول مورد نظر در سبد خرید یافت نشد", null);
        }
        #endregion

        #region Add Order Inforamtion
        [HttpGet("Add-Order-Inforamtion/{orderId}")]
        public async Task<IActionResult> AddorderInformation(int orderId)
        {
            var openOrder = await _orderService.GetOpenOrderUserForAddInformation(orderId, User.GetUserId());
            if (openOrder == null) return NoContent();
            ViewBag.UserAddress = await _addressService.GetUserAddressByUserId(User.GetUserId());
            return View(openOrder);
        }
        [HttpPost("Add-Order-Inforamtion/{orderId}")]
        public async Task<IActionResult> AddorderInformation(int orderId,CreateOrderInforamtionViewModel createOrderInforamtion)
        {
            var openOrder = await _orderService.GetOpenOrderUserForAddInformation(orderId, User.GetUserId());
            if (openOrder == null) return NoContent();
            if (ModelState.IsValid)
            {
                var res = await _orderService.CreateOrderInforamtion(createOrderInforamtion, User.GetUserId());
                switch (res)
                {
                    case CreateOrderInforamtionResult.Error:
                        TempData[ErrorMessage] = "خطا در ثبت اطلاعات";
                        break;
                    case CreateOrderInforamtionResult.NotFoundAddress:
                        TempData[ErrorMessage] = "آدرسی یافت نشد";
                        TempData[WarningMessage] = "کاربر گرامی با کلیک بر روی دکمه بالا آدرسی را برای ارسال محصول انتخاب بفرمایید ";
                        break;
                    case CreateOrderInforamtionResult.NotFoundUserOpenOrder:
                        TempData[ErrorMessage] = "سفارشی با این مشخصات یافت نشده ";
                        break;
                    case CreateOrderInforamtionResult.Success:
                        TempData[SuccessMessage] = "اطلاعات گیرنده با موفقیت ثبت شده ";
                        TempData[InfoMessage] = "کاربر گرامی اکنون شما می توانید سفارش خد را نهایی کنید";
                        return RedirectToAction("PayOrder");
                }
            }
            ViewBag.UserAddress = await _addressService.GetUserAddressByUserId(User.GetUserId());
            return View(openOrder);
        }
        #endregion

        #region Pay Order
        [HttpGet("PayOrder")]
        public async Task<IActionResult> PayOrder()
        {
            var openOrder = await _orderService.GetUserOpenOrderDetials(User.GetUserId());
            return View(openOrder);
        }

        #endregion

        #region FilterOrder

        [HttpGet("order-list")]
        public async Task<IActionResult> FilterOrder(FilterOrderViewModel filterOrder)
        {
            filterOrder.UserId = User.GetUserId();
            return View(await _orderService.FilterOrder(filterOrder));
        }

        #endregion
    }
}
