using ETicket.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicket.Domain
{
    public class ETicketDataContext : DbContext
    {
        public DbSet<TransactionHistory> TransactionHistory { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }

        public ETicketDataContext(DbContextOptions<ETicketDataContext> options) : base(options) { }

        public void EnsureCreated()
        {
            Database.EnsureCreated();
        }
    }
}