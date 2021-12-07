using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiStore.Domain.ViewModels.Address
{
    public class EditAddressViewModel
    {
        public int  AddressId { get; set; }
      [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string State { get; set; }
        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string City { get; set; }
        [Display(Name = "پلاک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MinLength(5, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
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
    public enum EditAddressResult
    {
        Success,
        Error,
        NotFound,
        NotFoundUser
    }
}
