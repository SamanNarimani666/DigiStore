using System.Collections.Generic;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.Product
{
    public class FilterProductViewModel:BasePaging
    {
        public string Name { get; set; }
        public int?  SellerId { get; set; }

        public List<Entities.Product> Products { get; set; }
        public FilterProductState FilterProductState { get; set; }
        #region Methods

        public FilterProductViewModel SetProduct(List<Entities.Product> products)
        {
            this.Products = products;
            return this;

        }
        public FilterProductViewModel SetPaging(BasePaging paging)
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
    public enum FilterProductState : byte
    {
        Active,
        NotActive,
        UnderProgress,
        Accepted,
        Rejected
    }
}
