using System.Collections.Generic;
using DigiStore.Domain.Enums.Ticket;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.Ticket
{
    public class FilterTicketViewModel:BasePaging
    {
        #region properties

        public string Title { get; set; }

        public long? UserId { get; set; }

        public TicketSection? TicketSection { get; set; }

        public TicketPriority? TicketPriority { get; set; }

        public FilterTicketOrder OrderBy { get; set; }

        public List<Entities.Ticket> Tickets { get; set; }

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
        CreateDate_DES,
        CreateDate_ASC,
    }
}
