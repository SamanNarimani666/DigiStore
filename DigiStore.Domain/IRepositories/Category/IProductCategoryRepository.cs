using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;

namespace DigiStore.Domain.IRepositories.Category
{
    public interface IProductCategoryRepository:IAsyncDisposable
    {
        Task<List<ProductCategory>> GetAllProductCategoriesByParentId(int? parentId);
        Task<List<ProductCategory>> GetAllActiveProductCategory();
        Task<ProductCategory> GetProductCategoryByCategoryId(int categoryId);
        void EditCategory(ProductCategory productCategory);
        Task AddCategory(ProductCategory productCategory);
        Task Save();

    }
}
