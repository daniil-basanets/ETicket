using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.WebAPI.Models.Interfaces;
using LiqPay.SDK;
using LiqPay.SDK.Dto;
using LiqPay.SDK.Dto.Enums;

namespace ETicket.WebAPI.LiqPayApi
{
    public class PaymentsService
    {
        #region Private Members

        private readonly LiqPayClient liqPayClient;

        private readonly ITransactionService transactionAppService;
        private readonly ITicketService ticketService;
        private readonly IUserService userService;

        #endregion

        public PaymentsService(
            ITransactionService transactionAppService,
            ITicketService ticketService,
            IUserService userService,
            IMerchant merchant
        )
        {
            this.userService = userService;
            this.ticketService = ticketService;
            this.userService = userService;
            this.transactionAppService = transactionAppService;

            liqPayClient = new LiqPayClient(merchant.MerchantId.ToString(), merchant.Password);
        }

        public async Task<LiqPayResponse> Process(
            decimal amount,
            string description,
            string card,
            string expirationMonth,
            string expirationYear,
            string cvv2,
            int ticketTypeId,
            string email
        )
        {
            var response = await RequestBuyTicket(amount, description, card, expirationMonth, expirationYear, cvv2);

            var transactionHistoryId = Guid.NewGuid();

            SaveTransaction(transactionHistoryId, response.PaymentId, response.Amount);
            SaveTicket(email, ticketTypeId, transactionHistoryId);

            return response;
        }

        private async Task<LiqPayResponse> RequestBuyTicket(
            decimal amount,
            string description,
            string card, 
            string expirationMonth, 
            string expirationYear, 
            string cvv2
        )
        {
            var requestParams = CreateLiqPayRequest(amount, description, card, expirationMonth, expirationYear, cvv2);

            var endpoint = "request";
            var response = await liqPayClient.RequestAsync(endpoint, requestParams);

            return response;
        }

        private CardLiqPayRequest CreateLiqPayRequest(
            decimal amount,
            string description,
            string card,
            string expirationMonth,
            string expirationYear,
            string cvv2
        )
        {
            return new CardLiqPayRequest
            {
                Action = LiqPayRequestAction.Pay,
                ActionPayment = LiqPayRequestActionPayment.Pay,
                Amount = (double)amount,
                Currency = LiqPayCurrency.UAH.GetAttributeOfType<EnumMemberAttribute>().Value,
                IsSandbox = true,
                Description = description,
                OrderId = $"{Guid.NewGuid()}",
                PayTypes = LiqPayRequestPayType.LiqPay,
                Language = LiqPayRequestLanguage.RU,
                Card = card,
                CardExpirationMonth = expirationMonth,
                CardExpirationYear = expirationYear,
                CardCvv2 = cvv2
            };
        }

        private void SaveTransaction(Guid transactionHistoryId, long referenceNumber, double totalPrice)
        {
            var transaction = new TransactionHistory
            {
                Id = transactionHistoryId,
                Date = DateTime.Now.Date,
                ReferenceNumber = $"{referenceNumber}",
                TotalPrice = (decimal)totalPrice
            };

            //transactionAppService.AddTransaction(transaction);
        }

        private void SaveTicket(string email, int ticketTypeId, Guid transactionHistoryId)
        {
            var userId = userService.GetByEmail(email).Id;

            var ticketDto = new TicketDto
            {
                Id = Guid.NewGuid(),
                CreatedUTCDate = DateTime.Now.Date,
                TransactionHistoryId = transactionHistoryId,
                TicketTypeId = ticketTypeId,
                UserId = userId
            };

            ticketService.Create(ticketDto);
        }
    }
}