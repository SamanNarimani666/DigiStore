using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.ProductFeature;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.ProductFeature
{
    public class ProductFeatureRepository : IProductFeatureRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public ProductFeatureRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddProductFeature
        public async Task AddProductFeature(List<Domain.Entities.ProductFeature> features)
        {
            await _context.ProductFeatures.AddRangeAsync(features);
        }
        #endregion


        #region RemoveProductFeature
        public void RemoveProductFeature(List<Domain.Entities.ProductFeature> features)
        {
            _context.ProductFeatures.RemoveRange(features);
        }
        #endregion

        #region GetProductFeatureByProductId
        public List<Domain.Entities.ProductFeature> GetProductFeatureByProductId(int productId)
        {
            return _context.ProductFeatures.Where(p => p.ProductId == productId).ToList();
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
