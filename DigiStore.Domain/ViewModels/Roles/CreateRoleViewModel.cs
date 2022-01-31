using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Roles
{
    public class CreateRoleViewModel
    {
        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]

        public string RoleTitle { get; set; }
    }

    public enum CreateRoleResult
    {
        Success,
        Error
    }
}
