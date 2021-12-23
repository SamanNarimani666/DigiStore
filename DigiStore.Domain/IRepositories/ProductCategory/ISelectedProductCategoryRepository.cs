using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;

namespace DigiStore.Domain.IRepositories.SelectedProductCategory
{
    public interface ISelectedProductCategoryRepository:IAsyncDisposable
    {
        Task AddSelectedProductCategory(List<ProductSelectedCategory> selectedCategory);
        Task Save();
    }
}
