using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.SalesOrderDetail;

namespace DigiStore.Data.Repositories.SalesOrderDetail
{
    public class SalesOrderDetailRepository: ISalesOrderDetailRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public SalesOrderDetailRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddOrderDetail
        public async Task AddOrderDetail(Domain.Entities.SalesOrderDetail orderDetail)
        {
            await _context.SalesOrderDetails.AddAsync(orderDetail);
        }
        #endregion

        #region UpdateSalesOrderDetail
        public void UpdateSalesOrderDetail(Domain.Entities.SalesOrderDetail orderDetail)
        {
            _context.SalesOrderDetails.Update(orderDetail);
        }
        #endregion

        #region DeleteSalesOrderDetail
        public void DeleteSalesOrderDetail(Domain.Entities.SalesOrderDetail orderDetail)
        {
            _context.SalesOrderDetails.Remove(orderDetail);
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
