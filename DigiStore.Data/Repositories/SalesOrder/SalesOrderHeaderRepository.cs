using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.SalesOrder;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.SalesOrder
{
    public class SalesOrderHeaderRepository: ISalesOrderHeaderRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public SalesOrderHeaderRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddSalesOrderHeader
        public async Task AddSalesOrderHeader(SalesOrderHeader orderHeader)
        {
            await _context.SalesOrderHeaders.AddAsync(orderHeader);
        }
        #endregion

        #region GetSalesOrderHeaderByUserId
        public async Task<SalesOrderHeader> GetSalesOrderHeaderByUserId(int userId)
        {
          return  await _context.SalesOrderHeaders
              .Include(p=>p.SalesOrderDetails)
              .ThenInclude(p=>p.Product)
              .ThenInclude(p=>p.ProductDiscounts)
              .Include(p => p.SalesOrderDetails)
              .ThenInclude(p=>p.Color)
              .SingleOrDefaultAsync(p => p.UserId == userId && !p.IsPaiy);
        }
        #endregion

        #region IsOpenSalesOrderHeaderByUser
        public async Task<bool> IsOpenSalesOrderHeaderByUser(int userId)
        {
            return await _context.SalesOrderHeaders.AnyAsync(p => p.UserId == userId && !p.IsPaiy);
        }
        #endregion

        #region  UpdateSalesOrderHeader
        public void UpdateSalesOrderHeader(SalesOrderHeader orderHeader)
        {
            _context.SalesOrderHeaders.Update(orderHeader);
        }
        #endregion

        #region Save
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        #endregion

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
