using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Product
{
    public class CreateProductCommnetViewModel
    {
        public int  ProductId { get; set; }
        [Display(Name ="عنوان دیدگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }
        [Display(Name = "دیدگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(900, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Comment { get; set; }

        [Display(Name = "کیفیت ساخت")]
        public byte ConstructionQuality { get; set; }
        [Display(Name = "ارزش خرید نسبت به قیمت")]
        public byte PurchaseValueRelativeToPrice { get; set; }
        [Display(Name = "نوآوری")]
        public byte Innovation { get; set; }
        [Display(Name = "ویژگی ها و قابلیت ها")]
        public byte FeaturesAndCapabilities { get; set; }
        [Display(Name = "سهولت استفاده")]
        public byte EaseOfUse { get; set; }
        [Display(Name = "طراحی و ظاهر")]
        public byte DesignAndAppearance { get; set; }
    }

    public enum CreateProductCommnetResult
    {
        Success,
        Error,
        NotFoundProduct
    }
}
