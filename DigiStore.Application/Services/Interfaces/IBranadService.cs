using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Brand;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Application.Services.Interfaces
{
   public interface IBranadService:IAsyncDisposable
    {
        Task<List<Domain.Entities.Brand>> GetAllBrands();
        Task<FilterBrandViewModel> FilterBrands(FilterBrandViewModel filterBrand);
        Task<CreateBrandResult> CreateBrand(CreateBrandViewModel brand,IFormFile brandLogo);
        Task<EditBrandViewModel> GetBrandInfoForEdit(int brandId);
        Task<EditBrandResult> EditBrand(EditBrandViewModel brand, IFormFile brandLogo);
    }
}
