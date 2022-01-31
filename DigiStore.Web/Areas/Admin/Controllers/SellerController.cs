using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Common;
using DigiStore.Domain.ViewModels.Seller;
using DigiStore.Web.Http;
using DigiStore.Web.Security;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Web.Areas.Admin.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class SellerController : AdminBaseController
    {
        #region Constructor
        private readonly ISellerService _sellerService;
        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }
        #endregion

        #region SellerRequestList
        [PermissionChecker(2)]
        public async Task<IActionResult> SellerRequests(FilterSellerViewModel filterSeller)
        {
            return View(await _sellerService.FilterSeller(filterSeller));
        }
        #endregion

        #region AcceptSellerRequest
        [PermissionChecker(3)]
        public async Task<IActionResult> AcceptSellerRequest(int id)
        {
            var result = await _sellerService.AcceptSellerRequest(id);
            if (result)
            {
                return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "درخواست مورد نطر با موفقیت ثبت شد",
                    null
                );
            }
            return JsonResponseStatus.SendStatus(
                JsonResponseStatusType.Danger,
                "اطلاعاتی با این مشخصات یافت نشد",
                null
            );
        }
        #endregion

        #region RejectSellerRequest
        [PermissionChecker(4)]
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectSellerRequest(RejectItemViewModel rejectItem)
        {
            var result = await _sellerService.RejectItem(rejectItem);
            if (result)
            {
                return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "درخواست مورد نطر  رد  شد",
                    rejectItem
                );
            }
            return JsonResponseStatus.SendStatus(
                JsonResponseStatusType.Danger,
                "اطلاعاتی با این مشخصات یافت نشد",
                null
            );
        }
        #endregion

        #region SellerDetials
        [PermissionChecker(5)]
        [HttpGet("seller-detials")]
        public async Task<IActionResult> SellerDetials(int sellerId)
        {
            var seller = await _sellerService.SellerDetailsBySellerId(sellerId);
            if (seller == null) return NotFound();
            return View(seller);
        }
        #endregion
    }
}
