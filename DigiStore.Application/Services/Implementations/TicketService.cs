using System;
using System.Threading.Tasks;
using DigiStore.Application.Security;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.Enums.Ticket;
using DigiStore.Domain.IRepositories.Ticket;
using DigiStore.Domain.ViewModels.Ticket;

namespace DigiStore.Application.Services.Implementations
{
    public class TicketService : ITicketService
    {
        #region Constructor
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketMessageRepository _ticketMessageRepository;
        public TicketService(ITicketRepository ticketRepository, ITicketMessageRepository ticketMessageRepository)
        {
            _ticketRepository = ticketRepository;
            _ticketMessageRepository = ticketMessageRepository;
        }
        #endregion

        #region AddUserTicket
        public async Task<AddTicketResult> AddUserTicket(AddTicketViewModel addTicket, int userId)
        {
            if (string.IsNullOrEmpty(addTicket.Text)) return AddTicketResult.Error;
            var ticket = new Ticket
            {
                UserId = userId,
                Title = addTicket.Title,
                IsReadByOwner = true,
                IsReadByAdmin = false,
                TicketPriority = (byte)addTicket.TicketPriority,
                TicketSection = (byte)addTicket.TicketSection,
                TicketState = (byte)TicketState.UnderProgress,
            };
            await _ticketRepository.AddTicket(ticket);
            await _ticketMessageRepository.Save();
            var newMessage = new TicketMessage
            {
                UserId = userId,
                TicketId = ticket.TicketId,
                Text = addTicket.Text
            };
            await _ticketMessageRepository.AddTicketMessage(newMessage);
            await _ticketMessageRepository.Save();
            return AddTicketResult.Success;
        }
        #endregion

        #region FilterTickets
        public async Task<FilterTicketViewModel> FilterTickets(FilterTicketViewModel filterTicketViewModel)
        {
            return await _ticketRepository.FilterTickets(filterTicketViewModel);
        }
        #endregion

        #region TicketDetailsForShow
        public async Task<TicketDetailViewModel> TicketDetailsForShow(int ticketId, int userId)
        {
            return await _ticketRepository.TicketDetailsForShow(ticketId, userId);
        }
        #endregion

        #region AnswerTicket
        public async Task<AnswerTicketResult> AnswerTicket(AnswerTicketViewModel answerTicket, int userId)
        {
            var ticket = await _ticketRepository.GetTicketById(answerTicket.TicketId);
            if (ticket == null) return AnswerTicketResult.NotFound;
            if (ticket.UserId != userId) return AnswerTicketResult.NotForUser;
            var ticketMessage = new TicketMessage
            {
                TicketId = ticket.TicketId,
                UserId = userId,
                Text = answerTicket.Text.SanitizeText()
            };
            await _ticketMessageRepository.AddTicketMessage(ticketMessage);
            await _ticketMessageRepository.Save();
            ticket.IsReadByOwner = true;
            ticket.IsReadByAdmin = false;
            _ticketRepository.EditTicket(ticket);
            await _ticketRepository.Save();
            return AnswerTicketResult.Success;
        }
        #endregion

        #region DeleteTicket
        public async Task<DeleteTicketResult> DeleteTicket(DeleteTicketViewModel deleteTicket, int userId)
        {
            var ticketMessage = await _ticketMessageRepository.GetTicketMessageById(deleteTicket.TicketMessageId);
            if (ticketMessage == null) return DeleteTicketResult.NotFoundTicketMessage;
            if (ticketMessage.TicketId != deleteTicket.TicketId) return DeleteTicketResult.NotFoundTicket;
            if (ticketMessage.UserId != userId) return DeleteTicketResult.NotFoundUser;
            try
            {
                _ticketMessageRepository.DeleteTicketMessage(ticketMessage);
                await _ticketMessageRepository.Save();
                return DeleteTicketResult.Success;
            }
            catch
            {

                return DeleteTicketResult.Error;
            }
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _ticketRepository.DisposeAsync();
            await _ticketMessageRepository.DisposeAsync();
        }
        #endregion
    }
}
