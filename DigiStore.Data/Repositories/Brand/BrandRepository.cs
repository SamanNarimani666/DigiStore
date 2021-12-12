using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.Brand;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.Brand
{
    public class BrandRepository: IBrandRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public BrandRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllBrands
        public async Task<List<Domain.Entities.Brand>> GetAllBrands()
        {
            return await _context.Brands.Where(p => !p.IsDelete).ToListAsync();
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
