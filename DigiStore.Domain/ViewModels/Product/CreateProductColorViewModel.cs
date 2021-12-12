
using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Product
{
    public class CreateProductColorViewModel
    {
        [Display(Name = "رنگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ColorName { get; set; }

        [Display(Name = "قیمت")]
        public int Price { get; set; }
    }
}
