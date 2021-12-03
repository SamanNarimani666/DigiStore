using Microsoft.AspNetCore.Mvc;

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
