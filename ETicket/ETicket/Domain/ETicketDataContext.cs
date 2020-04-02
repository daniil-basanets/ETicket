using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain
{
    public class ETicketDataContext : DbContext
    {
        public ETicketDataContext(DbContextOptions<ETicketDataContext> options): base(options)
        {

        }

        public void EnsureCreated()
        {
            Database.EnsureCreated();
        }
    }
}
