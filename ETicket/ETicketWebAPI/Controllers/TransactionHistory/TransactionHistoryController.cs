using System;
using System.Linq;
using System.Reflection;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.WebAPI.Controllers.TransactionHistory
{
    [Route("api/transactionhistory")]
    [ApiController]
    public class TransactionHistoryController : ControllerBase
    {
        #region Private Members

        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ITransactionService transactionService;
        private readonly ITicketService ticketService;
        private readonly IUserService userService;

        #endregion

        public TransactionHistoryController(ITransactionService transactionService, ITicketService ticketService, IUserService userService)
        {
            this.transactionService = transactionService;
            this.ticketService = ticketService;
            this.userService = userService;
        }

        // GET: api/Transactions
        [HttpPost]
        [Route("transactions")]
        public IActionResult GetTransactions(GetTransactionsRequest request)
        {
            logger.Info(nameof(TransactionHistoryController.GetTransactions));

            try
            {
                var user = userService.GetByEmail(request.Email);
                if (user == null)
                {
                    logger.Warn(nameof(GetTransactions) + " user is null");

                    return NotFound();
                }

                var transactionHistories = ticketService
                        .GetTickets()
                        .Where(t => t.UserId == user.Id)
                        .Select(t => t.TransactionHistory);

                var transactions = transactionHistories.Select(t => new { t.ReferenceNumber, t.TotalPrice, t.Date });

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return BadRequest();
            }
        }

        // GET: api/Transactions
        [HttpPost]
        [Route("createtransaction")]
        public IActionResult CreateTransaction(TransactionHistoryDto transactionHistoryDto)
        {
            logger.Info(nameof(TransactionHistoryController.CreateTransaction));

            try
            {
                transactionService.AddTransaction(transactionHistoryDto);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return BadRequest();
            }
        }
    }
}