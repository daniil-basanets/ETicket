using ETicket.DataAccess.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ETicket.DataAccess.Domain
{
    public class ETicketDataContext : IdentityDbContext
    {
        #region DbSets

        public DbSet<TransactionHistory> TransactionHistory { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> ETUsers { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<Carrier> Carriers { get; set; }
        public DbSet<RouteStation> RouteStations { get; set; }
        public DbSet<SecretCode> SecretCodes { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<PriceList> PriceList { get; set; }
        public DbSet<Area> Areas { get; set; }

        #endregion

        public ETicketDataContext(DbContextOptions<ETicketDataContext> options) : base(options) { }

        public void EnsureCreated()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ToAsk - inquire Roman concerning Tabulations

            modelBuilder.Entity<User>()
                   .HasOne(i => i.Privilege)
                   .WithMany()
                   .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                    .HasOne(i => i.Document)
                    .WithMany()
                    .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}