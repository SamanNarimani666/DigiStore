using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Address
{
    public class AddAddressViewModel
    {
        [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string State { get; set; }
        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string City { get; set; }
        [Display(Name = "پلاک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(5, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
        public string Zipcode { get; set; }
        [Display(Name = "کد پستی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(10, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
        public string PostalCode { get; set; }
        [Display(Name = "واحد")]
        public string Unit { get; set; }
        [Display(Name = "نشانی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(800, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Address { get; set; }
    }
    public enum AddAddressResult
    {
        Success,
        Error,
    }
}
