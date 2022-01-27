using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.SiteSetting;
using DigiStore.Domain.ViewModels.Slider;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Web.Areas.Admin.Controllers
{
    public class SiteSettingController : AdminBaseController
    {
        #region Constructor
        private readonly ISiteService _siteService;
        public SiteSettingController(ISiteService siteService)
        {
            _siteService = siteService;
        }
        #endregion

        #region SiteInformation
        public async Task<IActionResult> SiteInformation()
        {
            return View(await _siteService.GetDefaultSiteInformation());
        }
        #endregion

        #region EditSiteSetting
        [HttpGet("EditSiteSetting")]
        public async  Task<IActionResult> EditSiteSetting()
        {
            return View(await _siteService.GetDefaultSiteInformation());
        }
        [HttpPost("EditSiteSetting")]
        public async Task<IActionResult> EditSiteSetting(GetSiteInformation editSiteInformation)
        {
            if (ModelState.IsValid)
            {
                var res = await _siteService.EditSiteSetting(editSiteInformation);
                switch (res)
                {
                    case EditSiteSettingResult.Error:
                        TempData[ErrorMessage] = "خطا در ویرایش اطلاعات";
                        break;
                    case EditSiteSettingResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعاتی یافت نشد";
                        break;
                    case EditSiteSettingResult.Success:
                        TempData[SuccessMessage] = "اطلاعات با موفقیت ویرایش شد";
                        return RedirectToAction("SiteInformation");
                }
            }
            return View(editSiteInformation);
        }
        #endregion

        #region Slider
        [HttpGet("list-slider")]
        public async Task<IActionResult> ListSlider(FilterSliderViewModel filterSlider)
        {
            return View(await _siteService.FilterSlider(filterSlider));
        }
        #endregion

        #region Create Slider
        [HttpGet("create-slider")]
        public IActionResult CreateSlider()
        {
            return View();
        }
        [HttpPost("create-slider")]
        public async Task<IActionResult> CreateSlider(CreateSliderViewModel createSlider,IFormFile sliderImage)
        {
            if (ModelState.IsValid)
            {
                var res = await _siteService.CreateSlider(createSlider, sliderImage);
                switch (res)
                {
                    case CreateSliderResult.Error:
                        TempData[ErrorMessage] = "خطا در ثبت اسلاید";
                        break;
                    case CreateSliderResult.DisplayProrityIsExist:
                        TempData[WarningMessage] = "تصویری با این اولویت وجود دارد";
                        break;
                    case CreateSliderResult.ImageError:
                        TempData[ErrorMessage] = "تصویر آپلود شده نامعتبر می باشد";
                        break;
                    case CreateSliderResult.Sucess:
                        TempData[SuccessMessage] = "اسلایدر با موفقیت ثبت شد";
                        return RedirectToAction("ListSlider", "SiteSetting");
                }
            }
            return View();
        }
        #endregion
    }
}
