using System.IO;

namespace DigiStore.Application.Extensions
{
    public static class PathExtension
    {
        #region SiteUrl
        public static string SiteUrl = "https://localhost:44390";
        #endregion

        #region UploadImage
        public static string UploadImage = "/Images/Upload/";
        public static string UploadImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Upload/");
        #endregion

        #region UploadLogoStore
        public static string UploadLogo = "/Images/Store/Logo/origin/";
        public static string UploadLogoServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Store/Logo/origin/");

        public static string UploadLogoThumb = "/Images/Store/Logo/Thumb/";
        public static string UploadLogoThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Store/Logo/Thumb/");
        #endregion

        #region DefaultStoreLogo
        public static string DefaultStoreLogo = "/Images/defaults/logo.png";
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

        #region Product Image
        public static string ProductImageOrigin = "/Images/Product/origin/";
        public static string ProductImageOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Product/origin/");

        public static string ProductImageThumb = "/Images/Product/Thumb/";
        public static string ProductImageThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Product/Thumb/");
        #endregion
    }
}
