using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.IRepositories.SiteSetting;
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
        [HttpGet("list-Contact-us")]
        public async Task<IActionResult> Index(FilterContactUsViewModel filterContactUs)
        {
            return View(await _siteService.FilterContactUs(filterContactUs));
        }
    }
}
