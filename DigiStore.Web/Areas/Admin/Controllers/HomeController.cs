using DigiStore.Web.Security;
using Microsoft.AspNetCore.Mvc;
namespace DigiStore.Web.Areas.Admin.Controllers
{
    [PermissionChecker(1)]
    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
