using System.ComponentModel.DataAnnotations;
using DigiStore.Domain.ViewModels.Site;


namespace DigiStore.Domain.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "کلمه ی عبور فعلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string CurrentPassword { get; set; }

        [Display(Name = "کلمه ی عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار کلمه ی عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Compare("NewPassword", ErrorMessage = "کلمه های عبور مغایرت دارند")]
        public string ConfirmNewPassword { get; set; }
    }

    public enum ChangePasswordResult
    {
        Success,
        NotFound,
        UnCurrentPassword,
        Error
    }
}
