using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Category
{
    public class CreateCategoryViewModel
    {
        [Display(Name = "عنوان دسته")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CategoryTitle { get; set; }

        [Display(Name = "آیکون")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]

        public string CategoryIcon { get; set; }
    }

    public class CreateSubCategoryViewModel
    {
        public int? ParentId { get; set; }

        [Display(Name = "عنوان دسته")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CategoryTitle { get; set; }
    }
    public enum CreateCategoryResult
    {
        Success,
        Error
    }
}
