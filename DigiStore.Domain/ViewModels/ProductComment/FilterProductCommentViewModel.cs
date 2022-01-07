using System.Collections.Generic;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.ProductComment
{
    public class FilterProductCommentViewModel : BasePaging
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int SellerId { get; set; }
        public OrderBydate OrderBydate { get; set; }
        public List<Productcomment> ProductComments { get; set; }

        #region Methods
        public FilterProductCommentViewModel SetComment(List<Entities.Productcomment> productcomments)
        {
            this.ProductComments = productcomments;
            return this;
        }
        public FilterProductCommentViewModel SetPaging(BasePaging paging)
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

    public enum OrderBydate
    {
        Desc_Date,
        Desc_Asc
    }
}
