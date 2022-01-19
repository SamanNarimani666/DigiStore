using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.ViewModels.Slider;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.Slider
{
    public class SiteSliderRepository: ISiteSliderRepository
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
            return await _context.Sliders.Where(p => p.IsActive.Value).ToListAsync();
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
