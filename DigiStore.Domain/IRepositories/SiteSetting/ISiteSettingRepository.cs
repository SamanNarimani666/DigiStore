using System;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.SiteSetting
{
    public interface ISiteSettingRepository : IAsyncDisposable
    {
        Task<Entities.SiteSetting> GetDefaultSiteSetting();
    }
}
