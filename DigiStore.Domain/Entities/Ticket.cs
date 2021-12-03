using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class Ticket
    {
        public Ticket()
        {
            TicketMessages = new HashSet<TicketMessage>();
        }

        public int TicketId { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; }
        public byte? TicketState { get; set; }
        public byte? TicketSection { get; set; }
        public byte? TicketPriority { get; set; }
        public bool? IsReadByOwner { get; set; }
        public bool? IsReadByAdmin { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<TicketMessage> TicketMessages { get; set; }
    }
}
