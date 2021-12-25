using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.ProductColor;
using DigiStore.Domain.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

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
            await _context.Colors.AddRangeAsync(colors);
        }
        #endregion

        #region GetColorProductByProductId
        public List<Color> GetColorProductByProductId(int productId)
        {
            return _context.Colors.AsQueryable().Where(c => !c.IsDelete && c.ProductId == productId).ToList();
        }
        #endregion

        #region DeleteProductColor
        public void DeleteProductColor(List<Color> colors)
        {
             _context.Colors.RemoveRange(colors);
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
