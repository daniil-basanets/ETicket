using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using ETicket.Models.Interfaces;
using ETicket.PrivatBankApi;
using ETicket.PrivatBankApi.PrivatBank;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ETicket.Services.BuyTicket
{
    public class PaymentsAppService
    {
        #region Private Members

        private readonly IUnitOfWork eTitcketData;
        private readonly IMerchantSettings merchantSettings;
        private readonly PrivatBankApiClient privatBankApiClient;

        #endregion

        public PaymentsAppService(
            IUnitOfWork eTitcketData,
            IMerchantSettings merchantSettings,
            PrivatBankApiClient privatBankApiClient
        )
        {
            this.eTitcketData = eTitcketData;
            this.merchantSettings = merchantSettings;
            this.privatBankApiClient = privatBankApiClient;
        }

        public BuyTicketResponse Process(BuyTicketRequest buyTicketRequest)
        {
            // Get User privilege coefficient
            var privilegeCoef = GetUserPrivilegeCoefficient(buyTicketRequest.UserId);

            // Calculate price
            var totalPrice = GetTicketsTotalPrice(buyTicketRequest, privilegeCoef);

            // Process with Private
            var errorMessage = string.Empty;
            if (totalPrice != 0)
                errorMessage = SendTransaction(totalPrice);

            // Check if fail transaction
            if (!string.IsNullOrEmpty(errorMessage))
                return new BuyTicketResponse { ErrorMessage = errorMessage };

            var transactionHistoryId = Guid.NewGuid();

            // Save transaction
            SaveTransaction(buyTicketRequest, transactionHistoryId, totalPrice);

            // Save tickets
            SaveTickets(buyTicketRequest, transactionHistoryId);

            // Save to database
            eTitcketData.Save();

            return new BuyTicketResponse();
        }

        private decimal GetUserPrivilegeCoefficient(Guid userId)
        {
            var privilege = eTitcketData.Users
               .GetAll()
               .Include(p => p.Privilege)
               .Where(u => u.Id == userId)
               .Select(p => p.Privilege)
               .FirstOrDefault();

            return (privilege == null) ? 1M : privilege.Coefficient;
        }

        private decimal GetTicketsTotalPrice(
            BuyTicketRequest buyTicketRequest,
            decimal privilegeCoef
        )
        {
            var ticketPrice = eTitcketData.TicketTypes
                .GetAll()
                .Where(t => t.Id == buyTicketRequest.TicketTypeId)
                .Select(t => t.Price)
                .First();

            return ticketPrice * buyTicketRequest.Amount * privilegeCoef;
        }

        private string SendTransaction(decimal totalPrice)
        {
            var privatBankCardRequest = new SendToPrivatBankCardRequest
            {
                PaymentId = $"{Guid.NewGuid()}",
                CardNumber = merchantSettings.CardNumber,
                Amount = totalPrice,
                Currency = "UAH",
                Details = "Test"
            };

            var privatBankCardResponse = privatBankApiClient
                .ExecuteAsync<SendToPrivatBankCardRequest, SendToPrivatBankCardResponse>(privatBankCardRequest).Result;

            return privatBankCardResponse.Payment.Message;
        }

        private Guid SaveTransaction(
            BuyTicketRequest buyTicketRequest,
            Guid transactionHistoryId,
            decimal totalPrice
        )
        {
            var referenceNumber = GetRandomRefNumberTransaction();

            var transaction = new TransactionHistory
            {
                Id = transactionHistoryId,
                ReferenceNumber = referenceNumber,
                TotalPrice = totalPrice,
                Date = DateTime.UtcNow,
                TicketTypeId = buyTicketRequest.TicketTypeId,
                Count = buyTicketRequest.Amount
            };

            eTitcketData.TransactionHistory.Create(transaction);

            return transactionHistoryId;
        }

        private string GetRandomRefNumberTransaction()
        {
            var randomNumber = new Random();

            return randomNumber
                .Next(1, 999999999)
                .ToString()
                .PadLeft(13, '0');
        }

        private void SaveTickets(
            BuyTicketRequest buyTicketRequest,
            Guid transactionHistoryId
        )
        {
            var length = buyTicketRequest.Amount;
            for (var i = 0; i < length; i++)
            {
                var ticket = new Ticket
                {
                    Id = Guid.NewGuid(),
                    TicketTypeId = buyTicketRequest.TicketTypeId,
                    CreatedUTCDate = DateTime.UtcNow,
                    TransactionHistoryId = transactionHistoryId
                };

                eTitcketData.Tickets.Create(ticket);
            }
        }
    }
}