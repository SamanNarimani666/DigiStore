using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Brand;

namespace DigiStore.Domain.IRepositories.Brand
{
    public interface IBrandRepository:IAsyncDisposable
    {
        Task<List<Entities.Brand>> GetAllBrands();
        Task<FilterBrandViewModel> FilterBrands(FilterBrandViewModel filterBrand);
        Task AddBrand(Entities.Brand brand);
        void UpdateBrand(Entities.Brand brand);
        Task<Entities.Brand> GetBrandByBrandId(int brandId);
        Task Save();
    }
}
