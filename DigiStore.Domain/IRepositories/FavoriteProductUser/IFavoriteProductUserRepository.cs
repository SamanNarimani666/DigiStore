using System;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.FavoriteProductUser
{
    public interface IFavoriteProductUserRepository:IAsyncDisposable
    {
        Task AddFavoriteProductUser(Entities.FavoriteProductUser favoriteProductUser);
        void UpdateFavoriteProductUser(Entities.FavoriteProductUser favoriteProductUser);
        Task Save();
    }
}
