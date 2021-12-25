using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Common
{
    public class RejectItemViewModel
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "علت عدم تایید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RejectMessage { get; set; }
    }
}
