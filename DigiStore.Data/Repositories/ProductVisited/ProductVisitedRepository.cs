using System;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.ProductVisited;

namespace DigiStore.Data.Repositories.ProductVisited
{
    public class ProductVisitedRepository: IProductVisitedRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public ProductVisitedRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddProductVisited
        public  async Task AddProductVisited(Domain.Entities.ProductVisited productVisited)
        {
            await _context.ProductVisiteds.AddAsync(productVisited);
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
