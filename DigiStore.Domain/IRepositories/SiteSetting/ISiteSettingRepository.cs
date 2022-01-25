using System;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.SiteSetting
{
    public interface ISiteSettingRepository : IAsyncDisposable
    {
        Task<Entities.SiteSetting> GetDefaultSiteSetting();
        void EditSiteSetting(Entities.SiteSetting siteSetting);
        Task<Entities.SiteSetting> GetDefaultSiteSettingById(int siteSettingId);

        Task Save();
    }
}
