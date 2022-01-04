using System.Collections.Generic;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.ProductVisited
{
    public class FilterProductVisitedViewModel : BasePaging
    {
        public int UserId { get; set; }
        public List<Entities.ProductVisited> ProductVisiteds { get; set; }
        #region Methods

        public FilterProductVisitedViewModel SetProduct( List<Entities.ProductVisited>products)
        {
            this.ProductVisiteds = products;
            return this;

        }
        public FilterProductVisitedViewModel SetPaging(BasePaging paging)
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
