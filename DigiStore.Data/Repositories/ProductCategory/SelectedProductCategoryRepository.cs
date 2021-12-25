using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.SelectedProductCategory;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.SelectedProductCategory
{
    public class SelectedProductCategoryRepository: ISelectedProductCategoryRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public SelectedProductCategoryRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddSelectedProductCategory
        public async  Task AddSelectedProductCategory(List<ProductSelectedCategory> selectedCategory)
        {
            foreach (var item in selectedCategory)
            {
                await _context.ProductSelectedCategories.AddAsync(item);
            }
            
        }

        public List<ProductSelectedCategory> GetProductSelectedCategoryByProductId(int productId)
        {
            return _context.ProductSelectedCategories.Where(p => p.ProductId == productId).ToList();
        }

        public void DeleteProductSelectedCategory(List<ProductSelectedCategory> selectedCategory)
        {
            _context.ProductSelectedCategories.RemoveRange(selectedCategory);
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
