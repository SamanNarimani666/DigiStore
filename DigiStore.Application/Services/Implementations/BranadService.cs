using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Application.Extensions;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.Brand;
using DigiStore.Domain.ViewModels.Brand;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Application.Services.Implementations
{
    public class BranadService : IBranadService
    {
        #region Constructor
        private readonly IBrandRepository _brandRepository;
        public BranadService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        #endregion

        #region GetAllBrands
        public async Task<List<Brand>> GetAllBrands()
        {
            return await _brandRepository.GetAllBrands();
        }
        #endregion

        #region FilterBrands
        public async Task<FilterBrandViewModel> FilterBrands(FilterBrandViewModel filterBrand)
        {
            return await _brandRepository.FilterBrands(filterBrand);
        }
        #endregion

        #region CreateBrand
        public async Task<CreateBrandResult> CreateBrand(CreateBrandViewModel brand, IFormFile brandLogo)
        {
            var newBrand = new Brand()
            {
                BrandName = brand.BrandName
            };
            try
            {
                if (brandLogo != null && brandLogo.IsImage())
                {
                    var imageName = Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(brandLogo.FileName);
                    var res = brandLogo.AddImageToServer(imageName, PathExtension.BrandOriginServer, 100, 100,
                        PathExtension.BrandThumbServer);
                    if (res)
                    {
                        newBrand.Logo = imageName;
                    }
                }
                await _brandRepository.AddBrand(newBrand);
                await _brandRepository.Save();
                return CreateBrandResult.Success;
            }
            catch
            {
                return CreateBrandResult.Error;
            }
        }
        #endregion

        #region GetBrandInfoForEdit
        public async Task<EditBrandViewModel> GetBrandInfoForEdit(int brandId)
        {
            var brand = await _brandRepository.GetBrandByBrandId(brandId);
            if (brand == null) return null;
            return new EditBrandViewModel()
            {
                BrandName = brand.BrandName,
                BrandLogo = brand.Logo
            };
        }
        #endregion

        #region EditBrand
        public async Task<EditBrandResult> EditBrand(EditBrandViewModel brand, IFormFile brandLogo)
        {
            var mainBrand = await _brandRepository.GetBrandByBrandId(brand.BrandId);
            if (mainBrand == null) return EditBrandResult.NotFount;
            try
            {
                if (brandLogo != null && brandLogo.IsImage())
                {
                    var imageName = Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(brandLogo.FileName);
                    var res = brandLogo.AddImageToServer(imageName, PathExtension.BrandOriginServer, 100, 100,
                        PathExtension.BrandThumbServer);
                    if (res)
                    {
                        mainBrand.Logo = imageName;
                    }
                }

                mainBrand.BrandName = brand.BrandName;
                _brandRepository.UpdateBrand(mainBrand);
                await _brandRepository.Save();
                return EditBrandResult.Success;
            }
            catch
            {
                return EditBrandResult.Error;
            }

        }
        #endregion

        #region DeleteBrand
        public async Task<bool> DeleteBrand(int brandId)
        {
            var mainBrand = await _brandRepository.GetBrandByBrandId(brandId);
            if (mainBrand == null) return false;
            try
            {
                mainBrand.IsDelete = true;
                _brandRepository.UpdateBrand(mainBrand);
                await _brandRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region RestoreBrand
        public async Task<bool> RestoreBrand(int brandId)
        {
            var mainBrand = await _brandRepository.GetBrandByBrandId(brandId);
            if (mainBrand == null) return false;
            try
            {
                mainBrand.IsDelete = false;
                _brandRepository.UpdateBrand(mainBrand);
                await _brandRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _brandRepository.DisposeAsync();
        }
        #endregion
    }
}
