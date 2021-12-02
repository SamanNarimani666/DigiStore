using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigiStore.Web.Areas.UserPanel.Controllers
{
    public class HomeController : UserBaseController
    {
        #region  Dashboard
        [Route("")]
        public IActionResult Dashboard()
        {
            return View();
        }
        #endregion
    }
}
