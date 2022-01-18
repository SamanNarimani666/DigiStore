using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.SalesOrder;
using DigiStore.Domain.ViewModels.Order;
using DigiStore.Domain.ViewModels.Paging;
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
              .ThenInclude(p=>p.ProductDiscountUses)
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

        #region FilterOrder
        public async Task<FilterOrderViewModel> FilterOrder(FilterOrderViewModel filterOrder)
        {
            var query = _context.SalesOrderHeaders
                .Where(p=>p.IsPaiy)
                .OrderByDescending(p => p.ModifiedDate).AsQueryable();

            #region Filter
            if (filterOrder.UserId != null && filterOrder.UserId != 0)
            {
                query = query.Where(p => p.UserId == filterOrder.UserId);
            }
            #endregion

            #region paging
            var pager = Pager.Build(filterOrder.PageId, await query.CountAsync(), filterOrder.TakeEntity,
                filterOrder.HowManyShowPageAfterAndBefore);
            var allProduct = query.Paging(pager).ToList();
            return filterOrder.SetPaging(pager).SetOrder(allProduct);
            #endregion
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
