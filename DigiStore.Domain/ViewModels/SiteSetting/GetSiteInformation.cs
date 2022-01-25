using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.SiteSetting
{
    public class GetSiteInformation
    {
        public int SiteSettingId { get; set; }
        [Display(Name = "تلفن تماس")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Phone { get; set; }
        [Display(Name = "ایمیل")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Email { get; set; }
        [Display(Name = "کپی رایت")]
        public string CopyRight { get; set; }
        [Display(Name = "آدرس")]
        public string Address { get; set; }
        [Display(Name = "درباره ما")]
        public string AboutUs { get; set; }
        [Display(Name = "متن پایین صفحه")]
        public string FooterText { get; set; }
    }
    public enum EditSiteSettingResult
    {
        Success,
        Error,
        NotFound
    }
}
