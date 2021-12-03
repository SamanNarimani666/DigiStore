
using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Ticket
{
    public class AnswerTicketViewModel
    {
        public int  TicketId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; }
    }
    public enum AnswerTicketResult
    {
        NotForUser,
        NotFound,
        Success
    }
}
