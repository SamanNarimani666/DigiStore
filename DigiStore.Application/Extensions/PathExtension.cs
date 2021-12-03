using System.IO;

namespace DigiStore.Application.Extensions
{
    public static class PathExtension
    {
        #region SiteUrl
        public static string SiteUrl = "https://localhost:44390";
        #endregion

        #region DefaultAvatar
        public static string DefaultAvatar = "/Images/defaults/Default.jpg";
        #endregion

        #region user avatar
        public static string UserAvatarOrigin = "/Images/User/origin/";
        public static string UserAvatarOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/User/origin/");

        public static string UserAvatarThumb = "/Images/User/Thumb/";
        public static string UserAvatarThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/User/Thumb/");
        #endregion
    }
}
