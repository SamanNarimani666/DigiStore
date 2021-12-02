using System.ComponentModel.DataAnnotations;
using DigiStore.Domain.ViewModels.Site;

namespace DigiStore.Domain.ViewModels.Account
{
    public class LoginViewModel:CaptchaViewModel
    {
        [Display(Name = "تلفن همراه/ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string EmailOrMobiel { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PassWord { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
    public enum LoginResult
    {
        Success,
        NotFound,
        NotActive,
        UserIsBlock,
    }
}
