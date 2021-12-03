using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Ticket;

namespace DigiStore.Domain.IRepositories.Ticket
{
    public interface ITicketRepository : IAsyncDisposable
    {
        Task AddTicket(Entities.Ticket ticket);
        Task<FilterTicketViewModel> FilterTickets(FilterTicketViewModel filterTicketViewModel);
        Task<Entities.Ticket> GetTicketById(int ticketId);
        Task<TicketDetailViewModel> TicketDetailsForShow(int ticketId, int userId);
        void EditTicket(Entities.Ticket ticket);
        Task Save();
    }
}
