
using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Ticket;

namespace DigiStore.Application.Services.Interfaces
{
    public interface ITicketService:IAsyncDisposable
    {
        Task<AddTicketResult> AddUserTicket(AddTicketViewModel addTicket,int userId);
        Task<FilterTicketViewModel> FilterTickets(FilterTicketViewModel filterTicketViewModel);
        Task<TicketDetailViewModel> TicketDetailsForShow(int ticketId,int userId);
        Task<AnswerTicketResult> AnswerTicket(AnswerTicketViewModel answerTicket,int userId);
    }
}
