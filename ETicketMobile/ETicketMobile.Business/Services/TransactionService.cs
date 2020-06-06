using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Transactions;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Java.Net;

namespace ETicketMobile.Business.Services
{
    public class TransactionService : ITransactionService
    {
        #region Fields

        private readonly IHttpService httpService;

        #endregion

        public TransactionService(IHttpService httpService)
        {
            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string email)
        {
            var getTransactionsRequestDto = new GetTransactionsRequestDto { Email = email };

            try
            {
                var transacationsDto = await httpService.PostAsync<GetTransactionsRequestDto, IEnumerable<TransactionDto>>(
                        TransactionsEndpoint.GetTransactionsByEmail, getTransactionsRequestDto);

                var transactions = AutoMapperConfiguration.Mapper.Map<IEnumerable<Transaction>>(transacationsDto);

                return transactions;
            }
            catch (System.Net.WebException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
            catch (SocketException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
        }
    }
}