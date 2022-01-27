using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Account
{
    public class EditUserProfileViewModel 
    {
        [Display(Name = "نام ")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string LastName { get; set; }

        public string AvatarName { get; set; }
        
    }
    public enum EditUserProfileResult
    {
        Success,
        NotFound,
        NotActive,
        UserIsBlock,
        NotIsIamage,
        Error
    }
}
