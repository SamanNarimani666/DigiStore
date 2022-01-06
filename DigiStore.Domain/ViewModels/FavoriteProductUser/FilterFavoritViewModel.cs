using System.Collections.Generic;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.FavoriteProductUser
{
    public class FilterFavoritViewModel:BasePaging
    {
        public int UserId { get; set; }
        public List<Entities.FavoriteProductUser> FavoriteProductUsers { get; set; }
        #region Methods

        public FilterFavoritViewModel SetProduct(List<Entities.FavoriteProductUser> favoriteProductUsers)
        {
            this.FavoriteProductUsers = favoriteProductUsers;
            return this;

        }
        public FilterFavoritViewModel SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.PageCount = paging.PageCount;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            return this;
        }

        #endregion
    }
}
