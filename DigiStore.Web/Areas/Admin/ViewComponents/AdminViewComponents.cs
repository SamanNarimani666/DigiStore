using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Web.Areas.Admin.ViewComponents
{
  public class AdminUserInformation : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("AdminUserInformation");
        }
    }
  public class SidebarMenu:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SidebarMenu");
        }
    }
}
