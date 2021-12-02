using System.ComponentModel.DataAnnotations;
using DigiStore.Domain.ViewModels.Site;

namespace DigiStore.Application.ViewModels.Account
{
    public class ResetPsssWordViewModel:CaptchaViewModel
    {
        public string ActiveCode { get; set; }
        [Display(Name = "كلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }
        [Display(Name = "تكرار كلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Compare("Password", ErrorMessage = "کلمه های عبور مغایرت دارند")]
        public string ConferimPassWord { get; set; }
    }
    public enum ResetPsssWordResult
    {
        Success,
        NotFound
    }
}
