using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.Seller
{
    public class FilterSellerViewModel:BasePaging
    {
        #region properties

        public long? UserId { get; set; }
        public string StoreName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public FilterSellerState State { get; set; }

        public List<Entities.Seller>Sellers { get; set; }

        #endregion


        #region methods

        public FilterSellerViewModel SetSellers(List<Entities.Seller> sellers)
        {
            this.Sellers = sellers;
            return this;
        }

        public FilterSellerViewModel SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;
            return this;
        }

        #endregion
    }

    public enum FilterSellerState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "در حال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected
    }
}
