using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Web.HttpContext;
using Microsoft.AspNetCore.Authorization;

namespace DigiStore.Web.Areas.Seller.Controllers
{
    [Authorize]
    [Area("Seller")]
    [Route("Seller")]
    [CheckSellerState]
    public class SellerBaseController : Controller
    {
        protected string ErrorMessage = "ErrorMessage";
        protected string SuccessMessage = "SuccessMessage";
        protected string WarningMessage = "WarningMessage";
        protected string InfoMessage = "InfoMessage";
    }
}
