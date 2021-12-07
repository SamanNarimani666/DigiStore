using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.Ticket;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.Ticket
{
    public class TicketMessageRepository : ITicketMessageRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public TicketMessageRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion
        public async Task AddTicketMessage(TicketMessage ticketMessage)
        {
            await _context.TicketMessages.AddAsync(ticketMessage);
        }

        public void DeleteTicketMessage(TicketMessage ticketMessage)
        {
            _context.TicketMessages.Remove(ticketMessage);
        }

        public async Task<TicketMessage> GetTicketMessageById(int ticketMessageId)
        {
            return await _context.TicketMessages.FirstOrDefaultAsync(t => t.TicketMessageId == ticketMessageId);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }
        #endregion



    }
}
