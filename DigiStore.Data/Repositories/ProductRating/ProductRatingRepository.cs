using System;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.ProductRating;
using DigiStore.Domain.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

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

        #region GetProductRatingByProductId
        public async Task<ProductRatingViewModel> GetProductRatingByProductId(int productId)
        {
            var query = _context.ProductRatings.Where(p => p.ProductId == productId).AsQueryable();
            return new ProductRatingViewModel()
            {
                ProductId = productId,
                FeaturesAndCapabilities =  Convert.ToInt32( query.Average(p=>p.FeaturesAndCapabilities)),
                EaseOfUse = Convert.ToInt32(query.Average(p => p.EaseOfUse)),
                PurchaseValueRelativeToPrice = Convert.ToInt32(query.Average(p => p.PurchaseValueRelativeToPrice)),
                Innovation = Convert.ToInt32(query.Average(p => p.Innovation)),
                ConstructionQuality = Convert.ToInt32(query.Average(p => p.ConstructionQuality)),
                DesignAndAppearance = Convert.ToInt32(query.Average(p => p.DesignAndAppearance)),
                CountRating = await query.CountAsync()
            };
        }

        #endregion

        #region IsHaveProductRating
        public async Task<bool> IsHaveProductRating(int productId)
        {
            return await _context.ProductRatings.AnyAsync(p => p.ProductId == productId);
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
