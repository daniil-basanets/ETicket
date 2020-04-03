using System;
using System.Collections.Generic;
using ETicket.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.Domain.Entities
{
	public class PrivilegyRepository: IRepository<Privilege>
	{
        #region

        private readonly ETicketDataContext context;

        #endregion


    }
}
