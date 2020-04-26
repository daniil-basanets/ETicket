﻿using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.ApplicationServices.Services.Transaction
{
    public class TransactionAppService : ITransactionAppService
    {
        #region Private Members

        private readonly IUnitOfWork unitOfWork;

        #endregion

        public TransactionAppService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<TransactionHistory> Read()
        {
            return unitOfWork
                    .TransactionHistory
                    .GetAll()
                    .Include(t => t.TicketType)
                    .ToArray();
        }

        public TransactionHistory Read(Guid id)
        {
            return unitOfWork
                    .TransactionHistory
                    .GetAll()
                    .Include(t => t.TicketType)
                    .FirstOrDefault(t=> t.Id == id);
        }
    }
}