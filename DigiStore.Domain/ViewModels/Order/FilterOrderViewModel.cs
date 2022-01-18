using System.Collections.Generic;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.Order
{
    public class FilterOrderViewModel:BasePaging
    {
        public int UserId { get; set; }
        public List<SalesOrderHeader> SalesOrderHeaders { get; set; }

        #region Methods

        public FilterOrderViewModel SetOrder(List<SalesOrderHeader> salesOrderHeaders)
        {
            this.SalesOrderHeaders = salesOrderHeaders;
            return this;
        }
        public FilterOrderViewModel SetPaging(BasePaging paging)
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
