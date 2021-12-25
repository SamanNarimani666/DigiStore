using System.ComponentModel.DataAnnotations;


namespace DigiStore.Domain.ViewModels.Product
{
    public class RejectProductViewModel
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "علت عدم تایید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RejectMessage { get; set; }
        [Display(Name = "نام محصول")]
        public string ProductName { get; set; }

    }
}
