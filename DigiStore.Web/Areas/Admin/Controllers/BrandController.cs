using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Brand;
using Microsoft.AspNetCore.Mvc;
using DigiStore.Web.Areas.Admin.Controllers;
using DigiStore.Web.Http;
using DigiStore.Web.Security;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Web.Areas.Admin.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class BrandController : AdminBaseController
    {
        #region Constructor
        private readonly IBranadService _branadService;
        public BrandController(IBranadService branadService)
        {
            _branadService = branadService;
        }
        #endregion

        #region BrandList
        [PermissionChecker(16)]
        [HttpGet("brand-list")]
        public async Task<IActionResult> BrandList(FilterBrandViewModel filterBrand)
       {
           return View(await _branadService.FilterBrands(filterBrand));
       }
        #endregion

        #region CreateBrand
        [PermissionChecker(17)]
        [HttpGet("create-brands")]
        public  IActionResult CreateBrand()
        {
            return View();
        }
        [HttpPost("create-brands"),ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBrand(CreateBrandViewModel brand, IFormFile brandLogo)
        {
            if (ModelState.IsValid)
            {
                var res = await _branadService.CreateBrand(brand, brandLogo);
                switch (res)
                {
                    case CreateBrandResult.Error:
                        TempData[ErrorMessage] = "خطا در ثبت برند جدید";
                        break;
                    case CreateBrandResult.NotIsImage:
                        TempData[ErrorMessage] = "تصویر بارگذاری شده نامعتبر است";
                        break;
                    case CreateBrandResult.Success:
                        TempData[SuccessMessage] = "برند جدید با موفقیت ثبت شد";
                        return RedirectToAction("BrandList", "Brand");
                }
            }
            return View(brand);
        }
        #endregion

        #region EditBrand
        [PermissionChecker(18)]
        [HttpGet("edit-brand/{brandId}")]
        public async Task<IActionResult> EditBrand(int brandId)
        {
            var bradn = await _branadService.GetBrandInfoForEdit(brandId);
            if (bradn == null) return NoContent();
            return View("EditBrand",bradn);
        }
        [HttpPost("edit-brand/{brandId}")]
        public async Task<IActionResult> EditBrand(int brandId,EditBrandViewModel brand, IFormFile brandLogo)
        {
            if (ModelState.IsValid)
            {
                var res = await _branadService.EditBrand(brand, brandLogo);
                switch (res)
                {
                    case EditBrandResult.Error:
                        TempData[ErrorMessage] = "خطا در ویرایش برند";
                        break;
                    case EditBrandResult.NotFount:
                        TempData[ErrorMessage] = "برند مورد نظر بافت نشد";
                        break;
                    case EditBrandResult.Success:
                        TempData[SuccessMessage] = "برند مورد نظر با موفقیت ویرایش شد";
                        return RedirectToAction("BrandList", "Brand");
                }
            }
            var bradn = await _branadService.GetBrandInfoForEdit(brandId);
            if (bradn == null) return NotFound();
            return View("EditBrand",bradn);
        }
        #endregion

        #region DeleteBrand
        [PermissionChecker(19)]
        public async Task<IActionResult> RemoveBrand(int brandId)
        {
            var res = await _branadService.DeleteBrand(brandId);
            if (res)
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

        #region RestoreBrand
        [PermissionChecker(20)]
        public async Task<IActionResult> RestoreBrand(int brandId)
        {
            var res = await _branadService.RestoreBrand(brandId);
            if (res)
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
    }
}
