using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.Product;

namespace DigiStore.Data.Repositories.Product
{
    public class ProductRepository: IProductRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public ProductRepository(DigiStore_DBContext context)
        {
            _context = context;
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
