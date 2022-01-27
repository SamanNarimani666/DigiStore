using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Contacts
{
    public class ContactusForAnsweViewModel
    {
        public int ContactUsId { get; set; }
        [Display(Name = "آی پی")]
        public string UserIp { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "نام کامل")]
        public string FullName { get; set; }
        [Display(Name = "موضوع")]
        public string Subject { get; set; }
        [Display(Name = "متن")]
        public string Text { get; set; }

        [Display(Name = "جواب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string AnswerText { get; set; }
    }

    public enum AnswerContactusResult
    {
        Success,
        Error,
        TextIsEmpty,
        NotFound
    }
}
