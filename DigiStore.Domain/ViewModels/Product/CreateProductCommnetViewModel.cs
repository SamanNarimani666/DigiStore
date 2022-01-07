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
    }

    public enum CreateProductCommnetResult
    {
        Success,
        Error,
        NotFoundProduct
    }
}
