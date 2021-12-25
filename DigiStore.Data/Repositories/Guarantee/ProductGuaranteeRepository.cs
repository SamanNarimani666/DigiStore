using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.Guarantee;

namespace DigiStore.Data.Repositories.Guarantee
{
  public  class ProductGuaranteeRepository: IProductGuaranteeRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public ProductGuaranteeRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddProductGuarantee
        public async Task AddProductGuarantee(List<Domain.Entities.Guarantee> guarantees)
        {
            foreach (var guarantee in guarantees)
            {
                await _context.Guarantees.AddAsync(guarantee);
            }
        }
        #endregion

        #region GetProductGuaranteesByProductId
        public List<Domain.Entities.Guarantee> GetProductGuaranteesByProductId(int productId)
        {
            return _context.Guarantees.Where(g => g.ProductId == productId&& !g.IsDelete).ToList();
        }
        #endregion

        #region DeleteProductGuarantee
        public void DeleteProductGuarantee(List<Domain.Entities.Guarantee> guarantees)
        {
           _context.Guarantees.RemoveRange(guarantees);
        }
        #endregion

        #region Save
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        #endregion

        #region DisposeAsync
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
