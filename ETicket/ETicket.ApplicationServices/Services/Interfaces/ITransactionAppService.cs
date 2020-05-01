using System;
using System.Collections.Generic;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITransactionAppService
    {
        IEnumerable<TransactionHistory> Read();

        TransactionHistory Read(Guid id);
    }
}