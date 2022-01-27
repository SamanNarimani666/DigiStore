using System.ComponentModel.DataAnnotations;


namespace DigiStore.Domain.ViewModels.Slider
{
    public class CreateSliderViewModel
    {
        [Display(Name = "لینک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Link { get; set; }

        [Display(Name = "اولویت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public byte ImagepDisplayPrority { get; set; }

        [Display(Name = "فعال/غیر فعال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public bool IsActive  { get; set; }
    }
    public enum CreateSliderResult
    {
        Sucess,
        ImageError,
        DisplayProrityIsExist,
        Error
    }
}
