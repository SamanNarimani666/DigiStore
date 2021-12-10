using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Seller
{
    public class RejectItemViewModel
    {
        [Required]
        public int SellerId { get; set; }
        [Display(Name = "علت عدم تایید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RejectMessage { get; set; }
    }
}
