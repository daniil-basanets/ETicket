using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using ETicket.PrivatBankApi;
using ETicket.PrivatBankApi.PrivatBank;
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

        public async Task<BuyTicketResponse> ProcessAsync(
            BuyTicketRequest buyTicketRequest
        )
        {
            // Calculate price
            var ticketType = eTitcketData.TicketTypes.GetAll()
                .FirstOrDefault(x => x.Id == buyTicketRequest.TicketTypeId);

            var totalPrice = ticketType.Price * buyTicketRequest.Amount;

            // Process with Private
            await SendTransaction(totalPrice);

            // Save transaction
            var transactionHistoryId = Guid.NewGuid();
            SaveTransaction(buyTicketRequest, totalPrice, transactionHistoryId);

            // Save tickets
            SaveTickets(buyTicketRequest, transactionHistoryId);

            // Save to database
            eTitcketData.Save();

            return new BuyTicketResponse();
        }

        private async Task SendTransaction(decimal totalPrice)
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

            return;
        }

        private void SaveTransaction(
            BuyTicketRequest buyTicketRequest,
            decimal totalPrice,
            Guid transactionHistoryId
        )
        {
            var random = new Random();
            var referenceNumber = random.Next(1, 999999999).ToString().PadLeft(13, '0');

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