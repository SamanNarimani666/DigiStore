using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.ProductVisited;
using DigiStore.Domain.ViewModels.Paging;
using DigiStore.Domain.ViewModels.ProductVisited;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.ProductVisited
{
    public class ProductVisitedRepository : IProductVisitedRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public ProductVisitedRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddProductVisited
        public async Task AddProductVisited(Domain.Entities.ProductVisited productVisited)
        {
            await _context.ProductVisiteds.AddAsync(productVisited);
        }
        #endregion

        #region GetProductVisitedByProductIdAndUserId
        public async Task<Domain.Entities.ProductVisited> GetProductVisitedByProductIdAndUserId(int productId, int userId)
        {
            return await _context.ProductVisiteds.SingleOrDefaultAsync(p=>p.UserId==userId&&p.ProductId==productId);
        }
        #endregion

        #region filterFilterProductVisited
        public async Task<FilterProductVisitedViewModel> filterFilterProductVisited(FilterProductVisitedViewModel filterProductVisited)
        {
            var query = _context.ProductVisiteds.Where(p=>p.UserId==filterProductVisited.UserId)
                .Include(p => p.Product).OrderByDescending(p=>p.ModifiedDate).AsQueryable();
          
            var pager = Pager.Build(filterProductVisited.PageId, await query.CountAsync(), filterProductVisited.TakeEntity,
                filterProductVisited.HowManyShowPageAfterAndBefore);
            var allProduct = query.Paging(pager).ToList();
            return filterProductVisited.SetPaging(pager).SetProduct(allProduct);
        }
        #endregion

        #region IsVisitedUserThisProduct
        public async Task<bool> IsVisitedUserThisProduct(int productId, int userId)
        {
           return await _context.ProductVisiteds.AnyAsync(p=>p.ProductId==productId&&p.UserId==userId);
        }
        #endregion

        #region UpdateProductVisited
        public void UpdateProductVisited(Domain.Entities.ProductVisited productVisited)
        {
            _context.ProductVisiteds.Update(productVisited);
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
