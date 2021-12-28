using System.ComponentModel.DataAnnotations;
using DigiStore.Domain.ViewModels.Site;


namespace DigiStore.Domain.ViewModels.Account
{
   public class RegisterViewModel:CaptchaViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]

        public string UserName { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string Email { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(11, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
        [RegularExpression(@"09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}",ErrorMessage = "شماره تماس را درست وارد کنید")]
        public string Mobile { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(8, ErrorMessage = "{0} نمی تواند كمتر از {1} کاراکتر باشد .")]
       // [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,20}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        public string PassWord { get; set; }
        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(8, ErrorMessage = "{0} نمی تواند كمتر از {1} کاراکتر باشد .")]
        [Compare("PassWord", ErrorMessage = "کلمه های عبور مغایرت دارند")]
        public string ConferimPassWord { get; set; }

    }
   public enum RegisterResult
   {
       Success,
       ExistsEmail,
       ExistUserName,
       ExistMobile,
       Failed
   }
}
