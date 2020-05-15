using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETicket.DataAccess.Domain;
using ETicket.DataAccess.Domain.Entities;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.WebAPI.Models;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionHistoryController : ControllerBase
    {
        private readonly ITransactionAppService transactionService;
        private readonly ITicketService ticketService;
        private readonly IUserService userService;

        public TransactionHistoryController(
            ITransactionAppService transactionService,
            ITicketService ticketService,
            IUserService userService
        )
        {
            this.transactionService = transactionService;
            this.ticketService = ticketService;
            this.userService = userService;
        }

        // GET: api/Transactions
        [HttpPost]
        [Route("transactions")]
        public IEnumerable<TransactionHistory> GetTransactions(GetTransactionsRequest request)
        {
            var userId = userService.GetByEmail(request.Email).Id;
            var transactionHistories = ticketService
                    .GetTickets()
                    .Where(t => t.UserId == userId)
                    .Select(t => t.TransactionHistory);

            var transactions = transactionHistories.Select(t => new { t.ReferenceNumber, t.TotalPrice, t.Date });

            return transactionHistories;
        }
    }
}