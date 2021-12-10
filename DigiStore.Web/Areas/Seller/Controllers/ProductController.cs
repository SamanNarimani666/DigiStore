using Microsoft.AspNetCore.Mvc;
namespace DigiStore.Web.Areas.Seller.Controllers
{
    public class ProductController : Controller
    {
        #region ProductList
        public IActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
