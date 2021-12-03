using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;

namespace DigiStore.Domain.IRepositories.Ticket
{
    public interface ITicketMessageRepository:IAsyncDisposable
    {
        Task AddTicketMessage(TicketMessage ticketMessage);
        Task Save();
    }
}
