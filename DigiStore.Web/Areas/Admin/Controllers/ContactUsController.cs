using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Contacts;

namespace DigiStore.Web.Areas.Admin.Controllers
{
    public class ContactUsController : AdminBaseController
    {
        #region Constructor
        private readonly ISiteService _siteService;
        public ContactUsController(ISiteService siteService)
        {
            _siteService = siteService;
        }
        #endregion

        #region list-Contact-us
        [HttpGet("list-Contact-us")]
        public async Task<IActionResult> Index(FilterContactUsViewModel filterContactUs)
        {
            return View(await _siteService.FilterContactUs(filterContactUs));
        }
        #endregion

        #region Answer Contact Us
        [HttpGet("answer-contactus/{contactusId}")]
        public async Task<IActionResult> AnswerContactUs(int contactusId)
        {
            return View(await _siteService.GetContactusForAnswe(contactusId));
        }
        [HttpPost("answer-contactus/{contactusId}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerContactUs(int contactusId,ContactusForAnsweViewModel contactusForAnswe)
        {
            if (string.IsNullOrEmpty(contactusForAnswe.AnswerText))
            {
                TempData[WarningMessage] = "لطفا متن پاسخ را وارد نمایید";
            }

            if (ModelState.IsValid)
            {
                var res = await _siteService.AnswerContactUs(contactusForAnswe);
                switch (res)
                {
                    case AnswerContactusResult.Error:
                        TempData[ErrorMessage] = "خطا در عملیات ارسال ایمیل";
                        TempData[WarningMessage] = "لطفا بعد از دقایقی ایمیل را ارسال کنید";
                        break;
                    case AnswerContactusResult.NotFound:

                        TempData[ErrorMessage] = "مشخصاتی با این اطلاعات یافت نشد";
                        break;
                    case AnswerContactusResult.TextIsEmpty:
                        TempData[WarningMessage] = "لطفا متن پاسخ را وارد نمایید";
                        break;
                    case AnswerContactusResult.Success:
                        TempData[SuccessMessage] = "عملیات پاسخ به تماس با موفقیت انجام شد";
                        return RedirectToAction("Index", "ContactUs");
                }
            }
            return View(await _siteService.GetContactusForAnswe(contactusId));
        }
        #endregion
    }
}
