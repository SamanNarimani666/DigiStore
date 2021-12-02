using System.ComponentModel.DataAnnotations;
using DigiStore.Domain.ViewModels.Site;

namespace DigiMarket.Application.ViewModels.Account
{
    public class ForgotPassViewModel : CaptchaViewModel
    {
        [Display(Name = "ايميل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
    public enum ForgotPassResult
    {
        NotFount,
        FindUser,
        NotActive,
        UserIsBlock
    }
}
