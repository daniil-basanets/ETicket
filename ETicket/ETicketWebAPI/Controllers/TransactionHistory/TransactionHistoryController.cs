using System;
using System.Linq;
using System.Reflection;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ETicket.WebAPI.Controllers.TransactionHistory
{
    [Route("api/transactionhistory")]
    [ApiController]
    [SwaggerTag("Transaction service")]
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

        [HttpPost]
        [Route("transactions")]
        [SwaggerOperation(Summary = "Get all transactions", Description = "Allowed: authorized user")]
        [SwaggerResponse(200, "Returns if everything is right. Contains a list of transactions")]
        [SwaggerResponse(400, "Returns if an exseption occurred")]
        [SwaggerResponse(401, "Returns if user is unauthorized")]
        public IActionResult GetTransactions([FromBody, SwaggerRequestBody("Get transactions payload", Required = true)] GetTransactionsRequest request)
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

                var transactionHistories = transactionService.GetTransactionsByUserId(user.Id);

                var transactions = transactionHistories.Select(t => new { t.ReferenceNumber, t.TotalPrice, t.Date });

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return BadRequest();
            }
        }

        [HttpPost]
        [Route("transaction")]
        [SwaggerOperation(Summary = "Create new transaction", Description = "Allowed: authorized user")]
        [SwaggerResponse(200, "Returns if everything is right")]
        [SwaggerResponse(400, "Returns if an exseption occurred")]
        [SwaggerResponse(401, "Returns if user is unauthorized")]
        public IActionResult CreateTransaction([FromBody, SwaggerRequestBody("Transaction history payload", Required = true)] TransactionHistoryDto transactionHistoryDto)
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