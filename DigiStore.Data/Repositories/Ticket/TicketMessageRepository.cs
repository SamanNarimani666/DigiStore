using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.Ticket;

namespace DigiStore.Data.Repositories.Ticket
{
    public class TicketMessageRepository: ITicketMessageRepository
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

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
              await  _context.DisposeAsync();
            }
        }

      
    }
}
