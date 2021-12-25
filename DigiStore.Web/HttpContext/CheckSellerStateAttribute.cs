using DigiStore.Application.Services.Interfaces;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DigiStore.Web.HttpContext
{
    public class CheckSellerStateAttribute:AuthorizeAttribute,IAuthorizationFilter
    {
        private  ISellerService _sellerService;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _sellerService = (ISellerService)context.HttpContext.RequestServices.GetService(typeof(ISellerService));
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = context.HttpContext.User.GetUserId();
                if (!_sellerService.HasUserActiveSellerPanel(userId).Result)
                {
                    context.Result=new RedirectResult("/UserPanel");
                }
            }
        }
    }
}
