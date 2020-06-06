using System.Collections.Generic;
using System.Threading.Tasks;
using ETicketMobile.Business.Model.Transactions;

namespace ETicketMobile.Business.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactionsAsync(string email);
    }
}