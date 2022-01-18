using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Brand
{
    public class CreateBrandViewModel
    {
        [Display(Name = "کد پستی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BrandName { get; set; }
    }

    public enum CreateBrandResult
    {
        Success,
        Error
    }
}
