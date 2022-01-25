using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DigiStore.Domain.Enums.Ticket;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.Ticket
{
    public class FilterTicketViewModel:BasePaging
    {
        #region properties

        public string Title { get; set; }

        public int? UserId { get; set; }

        public FilterTicketSection FilterTicketSection { get; set; }


        public FilterTicketOrder OrderBy { get; set; }

        public List<Entities.Ticket> Tickets { get; set; }
        public FilterTicketPriority FilterTicketPriority { get; set; }
        #endregion
        #region Methods

        public FilterTicketViewModel SetTickets(List<Entities.Ticket> tickets)
        {
            this.Tickets = tickets;
            return this;

        }
        public FilterTicketViewModel SetPaging(BasePaging paging)
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

    public enum FilterTicketOrder
    {
        [Display(Name = "جدید")]
        CreateDate_DES,
        [Display(Name = "قدیمی")]
        CreateDate_ASC,
    }
    public enum FilterTicketPriority
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "کم")]
        Low,
        [Display(Name = "متوسط")]
        Medium,
        [Display(Name = "زیاد")]
        High
    }
    public enum FilterTicketSection
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "پشتیبانی")]
        Support,
        [Display(Name = "فنی")]
        Technical,
        [Display(Name = "فروش")]
        Sale
    }
}
