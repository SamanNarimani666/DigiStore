using System.Collections.Generic;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.ProductDiscount
{
    public class FilterProductDiscountViewModel:BasePaging
    {
        public int? ProductId { get; set; }
        public int?  SellerId { get; set; }
        public List<Entities.ProductDiscount> ProductDiscounts { get; set; }

        #region Methods

        public FilterProductDiscountViewModel SetDiscount(List<Entities.ProductDiscount> productDiscounts)
        {
            this.ProductDiscounts = productDiscounts;
            return this;

        }
        public FilterProductDiscountViewModel SetPaging(BasePaging paging)
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
