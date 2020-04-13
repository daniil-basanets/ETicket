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
            var totalPrice = GetTotalPrice(buyTicketRequest, privilegeCoef);

            // Process with Private
            var errorMessage = await SendTransaction(totalPrice);

            // Check if fail transaction
            if (!string.IsNullOrEmpty(errorMessage))
                return new BuyTicketResponse { ErrorMessage = errorMessage };

            // Save transaction
            var transactionHistoryId = Guid.NewGuid();
            SaveTransaction(buyTicketRequest, totalPrice, transactionHistoryId);

            // Save tickets
            SaveTickets(buyTicketRequest, transactionHistoryId);

            // Save to database
            eTitcketData.Save();

            return new BuyTicketResponse();
        }

        private decimal GetUserPrivilegeCoefficient(Guid userId)
        {
            var userPrivilege = eTitcketData.Users
               .GetAll()
               .Include(p => p.Privilege)
               .Where(u => u.Id == userId)
               .Select(p => p.Privilege)
               .First();

            return (userPrivilege == null) ? 1M : (decimal)userPrivilege.Coefficient;
        }

        private decimal GetTotalPrice(BuyTicketRequest buyTicketRequest, decimal privilegeCoef)
        {
            var ticketType = eTitcketData.TicketTypes.GetAll()
                .FirstOrDefault(x => x.Id == buyTicketRequest.TicketTypeId);

            return ticketType.Price * buyTicketRequest.Amount * privilegeCoef;
        }

        private async Task<string> SendTransaction(decimal totalPrice)
        {
            var cardNum = "4149439103721969";
            var privatBankCardRequest = new SendToPrivatBankCardRequest
            {
                PaymentId = Guid.NewGuid().ToString(),
                CardNumber = cardNum,
                Amount = totalPrice,
                Currency = "UAH",
                Details = "Test"
            };

            var privatBankCardResponse = await privatBankApiClient
                .ExecuteAsync<SendToPrivatBankCardRequest, SendToPrivatBankCardResponse>(privatBankCardRequest);

            return privatBankCardResponse.Payment.Message;
        }

        private void SaveTransaction(
            BuyTicketRequest buyTicketRequest,
            decimal totalPrice,
            Guid transactionHistoryId
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
        }

        private string GetRandomRefNumberTransaction()
        {
            var random = new Random();

            return random.Next(1, 999999999).ToString().PadLeft(13, '0');
        }

        private void SaveTickets(
            BuyTicketRequest buyTicketRequest,
            Guid transactionHistoryId
        )
        {
            var length = buyTicketRequest.Amount;
            for (int i = 0; i < length; i++)
            {
                var ticket = new Ticket
                {
                    Id = Guid.NewGuid(),
                    TicketTypeId = buyTicketRequest.TicketTypeId,
                    CreatedUTCDate = DateTime.UtcNow,
                    TransactionHistoryId = transactionHistoryId,
                };

                eTitcketData.Tickets.Create(ticket);
            }
        }
    }
}