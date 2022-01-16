using System;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.SalesInforamtion
{
    public interface ISalesInforamtionRepository : IAsyncDisposable
    {
        Task AddSalesInforamtion(Entities.SalesInforamtion salesInforamtion);
        Task Save();
    }
}
