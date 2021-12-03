using System.Collections.Generic;
using DigiStore.Domain.Entities;

namespace DigiStore.Domain.ViewModels.Ticket
{
    public class TicketDetailViewModel
    {
        public Entities.Ticket Ticket { get; set; }
        public List<TicketMessage> TicketMessage { get; set; }
    }
}
