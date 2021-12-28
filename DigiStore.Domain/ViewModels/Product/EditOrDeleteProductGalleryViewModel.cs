using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Product
{
    public class EditOrDeleteProductGalleryViewModel
    {
        [Display(Name = "اولویت تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public byte DisplayPrority { get; set; }
        public string ImageName { get; set; }
        public int GalleryId { get; set; }
        public int ProductId { get; set; }
    }

    public enum EditOrDeleteProductGalleryResult
    {
        Success,
        Error,
        NotForUserProduct,
        ImageIsNull,
        GalleryNotFound
    }
}
