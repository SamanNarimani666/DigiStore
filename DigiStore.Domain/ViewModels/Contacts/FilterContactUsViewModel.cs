using System.Collections.Generic;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.Contacts
{
    public class FilterContactUsViewModel : BasePaging
    {
        public string Email { get; set; }
        public List<ContactU> ContactUs { get; set; }

        #region Methods
        public FilterContactUsViewModel SetContactUs(List<ContactU> contactUs)
        {
            this.ContactUs = contactUs;
            return this;

        }
        public FilterContactUsViewModel SetPaging(BasePaging paging)
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
