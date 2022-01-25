using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Ticket
{
    public class DeleteTicketViewModel
    {
        [Required]
        public int  TicketId { get; set; }
        [Required]
        public int  TicketMessageId { get; set; }
        public int  UserId { get; set; }
    }

    public enum DeleteTicketResult
    {
        Success,
        Error,
        NotFoundUser,
        NotFoundTicket,
        NotFoundTicketMessage
    }
}
