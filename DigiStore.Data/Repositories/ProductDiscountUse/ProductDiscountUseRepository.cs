using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.ProductDiscountUse;

namespace DigiStore.Data.Repositories.ProductDiscountUse
{
    public class ProductDiscountUseRepository: IProductDiscountUseRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public ProductDiscountUseRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
        #endregion
    }
}
