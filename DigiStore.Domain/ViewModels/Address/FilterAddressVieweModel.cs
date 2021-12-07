using System.Collections.Generic;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.Address
{
    public class FilterAddressVieweModel : BasePaging
    {
        public int? UserId { get; set; }

        public List<Entities.Address> Addresses { get; set; }
        #region Methods

        public FilterAddressVieweModel SetAddress(List<Entities.Address> Addresses)
        {
            this.Addresses = Addresses;
            return this;

        }
        public FilterAddressVieweModel SetPaging(BasePaging paging)
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
