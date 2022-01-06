using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.FavoriteProductUser;

namespace DigiStore.Domain.IRepositories.FavoriteProductUser
{
    public interface IFavoriteProductUserRepository:IAsyncDisposable
    {
        Task<Entities.FavoriteProductUser> GetFavoriteProductUserById(int favoritId);
        Task AddFavoriteProductUser(Entities.FavoriteProductUser favoriteProductUser);
        void UpdateFavoriteProductUser(Entities.FavoriteProductUser favoriteProductUser);
        Task<bool> IsExistThisProductInUserFavoritList(int productId,int userId);
        Task<FilterFavoritViewModel> GetFavoriteProductUserByUserId(FilterFavoritViewModel filterFavorit);
        Task Save();
    }
}
