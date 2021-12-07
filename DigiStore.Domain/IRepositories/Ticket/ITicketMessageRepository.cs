using System;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;

namespace DigiStore.Domain.IRepositories.Ticket
{
    public interface ITicketMessageRepository:IAsyncDisposable
    {
        Task AddTicketMessage(TicketMessage ticketMessage);
        void DeleteTicketMessage(TicketMessage ticketMessage);
        Task<TicketMessage> GetTicketMessageById(int ticketMessageId);
        Task Save();
    }
}
