using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.ViewModels.Paging;
using DigiStore.Domain.ViewModels.Slider;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.Slider
{
    public class SiteSliderRepository : ISiteSliderRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public SiteSliderRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllActiveSlider
        public async Task<List<Domain.Entities.Slider>> GetAllActiveSlider()
        {
            return await _context.Sliders.Where(p => p.IsActive && !p.IsDelete).ToListAsync();
        }
        #endregion

        #region FilterSlider
        public async Task<FilterSliderViewModel> FilterSlider(FilterSliderViewModel filterSlider)
        {
            var query = _context.Sliders.AsQueryable();

            #region Order

            switch (filterSlider.SliderOrder)
            {
                case SliderOrder.Create_Date_Desc:
                    query = query.OrderByDescending(p => p.CreatedDate);
                    break;
                case SliderOrder.Create_Date_Asc:
                    query = query.OrderBy(p => p.CreatedDate);
                    break;
            }
            #endregion

            #region filter
            if (filterSlider.IsDelete)
            {
                query = query.Where(p => p.IsDelete);
            }
            #endregion

            #region Paging
            var pager = Pager.Build(filterSlider.PageId, await query.CountAsync(), filterSlider.TakeEntity,
                filterSlider.HowManyShowPageAfterAndBefore);
            var allTickets = query.Paging(pager).ToList();
            return filterSlider.SetPaging(pager).SetSlider(allTickets);
            #endregion
        }
        #endregion

        #region AddSlider
        public async Task AddSlider(Domain.Entities.Slider slider)
        {
            await _context.Sliders.AddAsync(slider);
        }
        #endregion

        #region CheckImageDisplayPrority
        public async Task<bool> CheckImageDisplayPrority(byte displayPrority)
        {
            return await _context.Sliders.AnyAsync(p =>p.DisplayPrority== displayPrority&& p.IsActive && !p.IsDelete);
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
