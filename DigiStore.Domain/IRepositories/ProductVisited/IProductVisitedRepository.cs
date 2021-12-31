using System;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.ProductVisited
{
    public interface IProductVisitedRepository:IAsyncDisposable
    {
        Task AddProductVisited(Entities.ProductVisited productVisited);
        Task Save();
    }
}
