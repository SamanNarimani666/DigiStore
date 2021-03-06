using System.IO;

namespace DigiStore.Application.Extensions
{
    public static class PathExtension
    {
        #region SiteUrl
        public static string SiteUrl = "https://www.samannarimani.ir";
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
        public static string DefaultAvatarThum = "/Images/defaults/DefaultThum.jpg";

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

        #region Product Gallery
        public static string ProductGalleryOrigin = "/Images/ProductGallery/origin/";
        public static string ProductGalleryOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ProductGallery/origin/");

        public static string ProductGalleryThumb = "/Images/ProductGallery/Thumb/";
        public static string ProductGalleryThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ProductGallery/Thumb/");
        #endregion

        #region Brand
        public static string BrandOrigin = "/Images/Brand/origin/";
        public static string BrandOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Brand/origin/");

        public static string BrandThumb = "/Images/Brand/Thumb/";
        public static string BrandThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Brand/Thumb/");

        #endregion

        #region Slider
        public static string SliderImageOrigin = "/Images/Slider/origin/";
        public static string SliderImageOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Slider/origin/");

        public static string SliderImageThumb = "/Images/Slider/Thumb/";
        public static string SliderImageThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Slider/Thumb/");
        #endregion
    }
}
