using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Order
{
    public class CreateOrderInforamtionViewModel
    {
        public int OrderId { set; get; }
        [Display(Name = "آدرس ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int AddressId { set; get; }
        [Display(Name = "نام گیرنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ReceiverName { get; set; }
        [Display(Name = "شماره موبایل گیرنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ReceiverMobile { get; set; }
        [Display(Name = "کد ملی گیرنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ReceiverNaationalId { get; set; }
        public int TotlaPrice { get; set; }
    }
    public enum CreateOrderInforamtionResult
    {
        Success,
        Error,
        NotFoundAddress,
        NotFoundUserOpenOrder
    }
}
