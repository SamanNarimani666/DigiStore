using Microsoft.AspNetCore.Mvc;


namespace DigiStore.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        public IActionResult Index()
        {
           
            return View();
        }

        public IActionResult Error404()=> View();
       
    }
}
