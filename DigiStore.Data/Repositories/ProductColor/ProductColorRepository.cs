using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.ProductColor;

namespace DigiStore.Data.Repositories.ProductColor
{
    public class ProductColorRepository : IProductColorRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public ProductColorRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddColor
        public async Task AddColor(List<Color> colors)
        {
            foreach (var color in colors)
            {
                await _context.Colors.AddAsync(color);

            }
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
