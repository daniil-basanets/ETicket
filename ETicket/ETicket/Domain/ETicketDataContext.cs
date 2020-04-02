using ETicket.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicket.Domain
{
    public class ETicketDataContext : DbContext
    {
        public DbSet<TransactionHistory> TransactionHistory { get; set; }

        public ETicketDataContext(DbContextOptions<ETicketDataContext> options): base(options) { }

        public void EnsureCreated()
        {
            Database.EnsureCreated();
        }
    }
}