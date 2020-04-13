using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using ETicket.PrivatBankApi;
using ETicket.PrivatBankApi.PrivatBank;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Services.BuyTicket
{
    public class PaymentsAppService
    {
        private readonly IUnitOfWork eTitcketData;
        private readonly PrivatBankApiClient privatBankApiClient;

        public PaymentsAppService(
            IUnitOfWork eTitcketData,
            PrivatBankApiClient privatBankApiClient
        )
        {
            this.eTitcketData = eTitcketData;
            this.privatBankApiClient = privatBankApiClient;
        }

        public async Task<BuyTicketResponse> ProcessAsync(BuyTicketRequest buyTicketRequest)
        {
            // Get User privilege coefficient
            var privilegeCoef = GetUserPrivilegeCoefficient(buyTicketRequest.UserId);

            // Calculate price
            var totalPrice = GetTicketsTotalPrice(buyTicketRequest, privilegeCoef);

            // Process with Private
            var errorMessage = await SendTransaction(totalPrice);

            // Check if fail transaction
            if (!string.IsNullOrEmpty(errorMessage))
                return new BuyTicketResponse { ErrorMessage = errorMessage };

            // Save transaction
            var transactionHistoryId = SaveTransaction(buyTicketRequest, totalPrice);

            // Save tickets
            SaveTickets(buyTicketRequest, transactionHistoryId);

            // Save to database
            eTitcketData.Save();

            return new BuyTicketResponse();
        }

        private decimal GetUserPrivilegeCoefficient(Guid userId)
        {
            var coefficient = eTitcketData.Users
               .GetAll()
               .Include(p => p.Privilege)
               .Where(u => u.Id == userId)
               .Select(p => p.Privilege.Coefficient)
               .FirstOrDefault();

            return coefficient == 0 ? 1M : coefficient;
        }

        private decimal GetTicketsTotalPrice(
            BuyTicketRequest buyTicketRequest,
            decimal privilegeCoef
        )
        {
            var ticketPrice = eTitcketData.TicketTypes
                .GetAll()
                .Where(t => t.Id == buyTicketRequest.TicketTypeId)
                .Select(x => x.Price)
                .First();

            return ticketPrice * buyTicketRequest.Amount * privilegeCoef;
        }

        private async Task<string> SendTransaction(decimal totalPrice)
        {
            var cardNum = "4149439103721969";
            var privatBankCardRequest = new SendToPrivatBankCardRequest
            {
                PaymentId = $"{Guid.NewGuid()}",
                CardNumber = cardNum,
                Amount = totalPrice,
                Currency = "UAH",
                Details = "Test"
            };

            var privatBankCardResponse = await privatBankApiClient
                .ExecuteAsync<SendToPrivatBankCardRequest, SendToPrivatBankCardResponse>(privatBankCardRequest);

            return privatBankCardResponse.Payment.Message;
        }

        private Guid SaveTransaction(
            BuyTicketRequest buyTicketRequest,
            decimal totalPrice
        )
        {
            var transactionHistoryId = Guid.NewGuid();
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
            var random = new Random();

            return random
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