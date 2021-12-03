﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.Ticket;
using DigiStore.Domain.ViewModels.Paging;
using DigiStore.Domain.ViewModels.Ticket;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.Ticket
{
    public class TicketRepository : ITicketRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public TicketRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddTicket
        public async Task AddTicket(Domain.Entities.Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
        }
        #endregion

        #region FilterTickets
        public async Task<FilterTicketViewModel> FilterTickets(FilterTicketViewModel filterTicketViewModel)
        {
            var ticket = _context.Tickets.AsQueryable();

            #region Order
            switch (filterTicketViewModel.OrderBy)
            {
                case FilterTicketOrder.CreateDate_ASC:
                    ticket = ticket.OrderBy(t => t.CreatedDate);
                    break;
                case FilterTicketOrder.CreateDate_DES:
                    ticket = ticket.OrderByDescending(t => t.CreatedDate);
                    break;
            }
            #endregion

            #region filter
            if (filterTicketViewModel.TicketSection != null)
                ticket = ticket.Where(t => t.TicketSection == (byte)filterTicketViewModel.TicketSection.Value);
            if (filterTicketViewModel.TicketPriority != null)
                ticket = ticket.Where(t => t.TicketPriority == (byte)filterTicketViewModel.TicketPriority.Value);
            if (filterTicketViewModel.UserId != null)
                ticket = ticket.Where(t => t.UserId == filterTicketViewModel.UserId);
            if (!string.IsNullOrEmpty(filterTicketViewModel.Title))
                ticket = ticket.Where(t => EF.Functions.Like(t.Title, $"%{filterTicketViewModel.Title}%"));
            #endregion

            var pager = Pager.Build(filterTicketViewModel.PageId,await ticket.CountAsync(), filterTicketViewModel.TakeEntity,
                filterTicketViewModel.HowManyShowPageAfterAndBefore);
            var allTickets = ticket.Paging(pager).ToList();
            return filterTicketViewModel.SetPaging(pager).SetTickets(allTickets);
        }
        #endregion

        #region GetTicketById
        public async Task<Domain.Entities.Ticket> GetTicketById(int ticketId)
        {
            return await _context.Tickets.FindAsync(ticketId);
        }
        #endregion

        #region TicketDetailsForShow
        public async Task<TicketDetailViewModel> TicketDetailsForShow(int ticketId, int userId)
        {
            var ticket = await _context.Tickets
                .Include(t=>t.User)
                .Include(t=>t.TicketMessages)
                .SingleOrDefaultAsync(t=>t.TicketId==ticketId);
            if ((ticket == null) || (ticket.UserId != userId)) return null;
            return new TicketDetailViewModel
            {
                Ticket = ticket,
                TicketMessage = await _context.TicketMessages
                    .OrderBy(t=>t.CreatedDate)
                    .Where(t=>t.TicketId==ticketId).ToListAsync()
            };
        }
        #endregion

        #region EditTicket
        public void EditTicket(Domain.Entities.Ticket ticket)
        {
            _context.Tickets.Update(ticket);
        }
        #endregion

        #region Save
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        #endregion

        #region DisposeAsync
        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }
        #endregion

    }
}
