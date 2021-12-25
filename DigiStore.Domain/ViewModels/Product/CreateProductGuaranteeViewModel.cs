using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Product
{
    public class CreateProductGuaranteeViewModel
    {
        [Display(Name = "نام گارانتی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string GuaranteeName { get; set; }

        [Display(Name = "قیمت")]
        public int Price { get; set; }
    }
}
