using Microsoft.AspNetCore.Mvc;
namespace DigiStore.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
