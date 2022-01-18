using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.ProductRating;

namespace DigiStore.Data.Repositories.ProductRating
{
    public class ProductRatingRepository: IProductRatingRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public ProductRatingRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddProductRating
        public async Task AddProductRating(Domain.Entities.ProductRating productRating)
        {
            await _context.AddAsync(productRating);
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
