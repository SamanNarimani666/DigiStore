using System;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.SiteSetting;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.SiteSetting
{
    public class SiteSettingRepository: ISiteSettingRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public SiteSettingRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region GetDefaultSiteSetting
        public async Task<Domain.Entities.SiteSetting> GetDefaultSiteSetting()
        {
            return await _context.SiteSettings.FirstOrDefaultAsync(s => s.IsDefault.Value);
        }
        #endregion

        #region EditSiteSetting
        public void EditSiteSetting(Domain.Entities.SiteSetting siteSetting)
        {
            siteSetting.ModifiedDate = DateTime.Now;
            _context.SiteSettings.Update(siteSetting);
        }
        #endregion

        #region GetDefaultSiteSetting
        public async Task<Domain.Entities.SiteSetting> GetDefaultSiteSettingById(int siteSettingId)
        {
            return await _context.SiteSettings.FirstOrDefaultAsync(s =>s.SiteSettingId==siteSettingId&& s.IsDefault.Value);
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
