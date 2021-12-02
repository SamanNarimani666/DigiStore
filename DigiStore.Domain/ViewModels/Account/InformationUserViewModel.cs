using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Account
{
    public class InformationUserViewModel
    {
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "شماره تماس")]
        public string Mobile { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }
        [Display(Name = "تاریخ عضویت")]
        public string CreateData { get; set; }
  
    }
}
