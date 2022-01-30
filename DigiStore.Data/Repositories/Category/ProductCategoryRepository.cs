using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.Category;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.Category
{
  public  class ProductCategoryRepository: IProductCategoryRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public ProductCategoryRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllProductCategoriesByParentId
        public async Task<List<ProductCategory>> GetAllProductCategoriesByParentId(int? parentId)
        {
            if (parentId == null || parentId==0)
            {
                return await _context.ProductCategories
                    .AsQueryable()
                    .Where(c => !c.IsDelete && c.IsActive && parentId==null)
                    .ToListAsync();
            }
            return await _context.ProductCategories
                .AsQueryable()
                .Where(c => !c.IsDelete && c.IsActive&&c.ParentId==parentId)
                .ToListAsync();
        }
        #endregion

        #region GetAllProductCategory
        public async Task<List<ProductCategory>> GetAllActiveProductCategory()
        {
            return await _context.ProductCategories.Where(p=>p.IsActive&& !p.IsDelete).ToListAsync();
        }
        #endregion

        #region GetProductCategoryByCategoryId
        public async Task<ProductCategory> GetProductCategoryByCategoryId(int categoryId)
        {
            return await _context.ProductCategories.SingleOrDefaultAsync(p => p.ProductCategoryId == categoryId);
        }
        #endregion

        #region EditCategory
        public void EditCategory(ProductCategory productCategory)
        {
            productCategory.ModifiedDate=DateTime.Now;
            _context.ProductCategories.Update(productCategory);
        }
        #endregion

        #region AddCategory
        public async Task AddCategory(ProductCategory productCategory)
        {
            await _context.ProductCategories.AddAsync(productCategory);
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
