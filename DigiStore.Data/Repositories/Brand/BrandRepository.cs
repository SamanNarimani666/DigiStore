using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.Brand;
using DigiStore.Domain.ViewModels.Brand;
using DigiStore.Domain.ViewModels.Paging;
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

        #region FilterBrands
        public async Task<FilterBrandViewModel> FilterBrands(FilterBrandViewModel filterBrand)
        {
            var query = _context.Brands.AsQueryable();
            if (filterBrand.BrandTitle != null)
                query = query.Where(p => EF.Functions.Like(p.BrandName, $"%{filterBrand.BrandTitle}%"));
            #region paging
            var pager = Pager.Build(filterBrand.PageId, await query.CountAsync(), filterBrand.TakeEntity,
                filterBrand.HowManyShowPageAfterAndBefore);
            var allProduct = query.Paging(pager).ToList();
            return filterBrand.SetPaging(pager).SetBrand(allProduct);
            #endregion
        }
        #endregion

        #region AddBrands
        public async Task AddBrand(Domain.Entities.Brand brand)
        {
            await _context.Brands.AddAsync(brand);
        }
        #endregion

        #region UpdateBrand
        public void UpdateBrand(Domain.Entities.Brand brand)
        {
            brand.ModifiedDate=DateTime.Now;
            _context.Brands.Update(brand);
        }
        #endregion

        #region GetBrandByBrandId
        public async Task<Domain.Entities.Brand> GetBrandByBrandId(int brandId)
        {
            return await _context.Brands.SingleOrDefaultAsync(p => p.BrandId == brandId);
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
