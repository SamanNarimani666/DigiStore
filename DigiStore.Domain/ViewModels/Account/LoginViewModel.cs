using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiStore.Domain.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name = "تلفن همراه/ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string EmailOrMobiel { get; set; }


        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(8, ErrorMessage = "{0} نمی تواند كمتر از {1} کاراکتر باشد .")]
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
