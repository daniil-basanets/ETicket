using ETicket.DataAccess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class PriceListRepository
    {
        #region 

        private readonly ETicketDataContext context;

        #endregion

        public PriceListRepository(ETicketDataContext eTicketDataContext)
        {
            context = eTicketDataContext;
        }

        public void Create(PriceList item)
        {
            context.PriceList.Add(item);
        }

        public PriceList Get(int id)
        {
            return context.PriceList.FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<PriceList> GetAll()
        {
            return context.PriceList;
        }
    }
}
