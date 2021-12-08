using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigiStore.Application.Extensions;
using DigiStore.Application.Generators;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Seller;
using DigiStore.Web.Extensions;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Web.Areas.UserPanel.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class SellerController : UserBaseController
    {
        #region Constructor
        private readonly ISellerService _sellerService;
        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }
        #endregion

        #region SellerRequestList
        [HttpGet("seller-requests")]
        public async Task<IActionResult> SellerRequests(FilterSellerViewModel filterSeller)
        {
            filterSeller.UserId = User.GetUserId();
            filterSeller.TakeEntity = 5;
            filterSeller.State = FilterSellerState.All;
            return View(await _sellerService.FilterSeller(filterSeller));
        }

        #endregion

        #region RequestSellerPanel
        [HttpGet("request-seller-panel")]
        public IActionResult RequestSellerPanel()
        {
            return View();
        }
        [HttpPost("request-seller-panel"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestSellerPanel(RequestSellerViewModel requestSeller, IFormFile Logo, IFormFile SellerDocument)
        {
            if (ModelState.IsValid)
            {
                if (Logo != null && Logo.IsImage())
                {
                    var imageName = Generators.GeneratorsUniqueCode() + Path.GetExtension(Logo.FileName);
                    Logo.AddImageToServer(imageName, PathExtension.UploadLogoServer, 100, 100, PathExtension.UploadLogoThumbServer);
                    requestSeller.SellerLogo = imageName;
                }
                if (SellerDocument != null)
                {
                    var selletdocumentName = Generators.GeneratorsUniqueCode() + Path.GetExtension(SellerDocument.FileName);
                    SellerDocument.AddFileToServer(selletdocumentName, "wwwroot/Files/Store/SellerDocument/");
                    requestSeller.SellerDoucemnt = selletdocumentName;
                }
                var res = await _sellerService.AddNewSellerRequet(requestSeller, User.GetUserId());
                switch (res)
                {
                    case RequestSellerResult.Erorr:
                        TempData[ErrorMessage] = "خطا در ثبت اطلاعات فوشگاه";
                        break;
                    case RequestSellerResult.HasNotPermission:
                        TempData[WarningMessage] = "شما دسترسی لازم جهت انجام فرایند مورد نطر را ندارید";
                        break;
                    case RequestSellerResult.HasUnderProgressRequest:
                        TempData[WarningMessage] = "در خواست های قبلی شما در حال پیگیری می باشند ";
                        TempData[InfoMessage] = "در حال حاضر نمی توانید در خواست جدید ثبت کنید";
                        break;
                    case RequestSellerResult.Success:
                        TempData[SuccessMessage] = "درخواست شما جهت فروشندگی  در دیجی استور با موفقیت ثبت شد ";
                        TempData[InfoMessage] = $"  بعد از برسی اسناد و درخواست شما نتیجه اعلام خواهد شد با تشکر{User.Identity.Name} کاربر گرامی ";
                        return RedirectToAction("SellerRequests", "Seller", new { area = "UserPanel" });
                }
            }
            return View(requestSeller);
        }
        #endregion

        #region EditSellerRequest

        [HttpGet("edit-request-seller{id}")]
        public async Task<IActionResult> EditSellerRequest(int id)
        {
            var seller = await _sellerService.GetEditRequestSellerInfo(id, User.GetUserId());
            if (seller == null) return NotFound();
            return View(seller);
        }
        [HttpPost("edit-request-seller{id}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSellerRequest(EditRequestSellerViewModel editRequestSeller, IFormFile Logo, IFormFile SellerDocument)
        {
            if (ModelState.IsValid)
            {
                if (Logo != null && Logo.IsImage())
                {
                    var imageName = Generators.GeneratorsUniqueCode() + Path.GetExtension(Logo.FileName);
                    Logo.AddImageToServer(imageName, PathExtension.UploadLogoServer, 100, 100, PathExtension.UploadLogoThumbServer, editRequestSeller.SellerLogo);
                    editRequestSeller.SellerLogo = imageName;
                }
                if (SellerDocument != null)
                {
                    var selletdocumentName = Generators.GeneratorsUniqueCode() + Path.GetExtension(SellerDocument.FileName);
                    SellerDocument.AddFileToServer(selletdocumentName, "wwwroot/Files/Store/SellerDocument/", "wwwroot/Files/Store/SellerDocument/"+editRequestSeller.SellerDoucemnt);
                    editRequestSeller.SellerDoucemnt = selletdocumentName;
                }
                var res = await _sellerService.EditSellerRequest(editRequestSeller, User.GetUserId());
                switch (res)
                {
                    case EditRequestSellerResult.Erorr:
                        TempData[ErrorMessage] = "خطا در ویرایش اطلاعات";
                        break;
                    case EditRequestSellerResult.NotFound:
                        TempData[WarningMessage] = "درخواستی با این مشخصات یافت نشد";
                        break;
                    case EditRequestSellerResult.Success:
                        TempData[SuccessMessage] = "اطلاعات فروشگاه شما با موفقیت ویرایش شد ";
                        TempData[InfoMessage] = "بعد از برسی اطلاعات ویرایش شده فروشگاه شما فروشگاه شما فعال خواهد شد";
                        return RedirectToAction("SellerRequests", "Seller", new { area = "UserPanel" });
                }
            }
            return View(editRequestSeller);
        }
        #endregion
    }
}
