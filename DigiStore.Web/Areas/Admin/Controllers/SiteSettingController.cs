using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.SiteSetting;

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
    }
}
