using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Seller;
using DigiStore.Web.Http;
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
        public async Task<IActionResult> SellerRequests(FilterSellerViewModel filterSeller)
        {
            return View(await _sellerService.FilterSeller(filterSeller));
        }
        #endregion

        #region AcceptSellerRequest
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

        #region AcceptSellerRequest
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
    }
}
