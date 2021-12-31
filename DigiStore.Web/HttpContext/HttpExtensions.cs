namespace DigiStore.Web.HttpContext
{
    public static class HttpExtensions
    {
        public static string GetUserIp(this Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
