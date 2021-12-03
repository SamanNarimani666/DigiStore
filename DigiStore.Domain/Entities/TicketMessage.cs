using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class TicketMessage
    {
        public int TicketMessageId { get; set; }
        public int? TicketId { get; set; }
        public int? UserId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual User User { get; set; }
    }
}
