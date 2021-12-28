using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Product
{
    public class CreateProductGalleryViewModel
    {
        [Display(Name = "اولویت تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public byte DisplayPrority { get; set; }

    }

    public enum CreateProductGalleryResult
    {
        Success,
        Error,
        NotForUserProduct,
        ImageIsNull,
        ProductNotFound
    }
}
