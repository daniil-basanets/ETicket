using DBContextLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DBContextLibrary.Domain
{
    public class ETicketDataContext : DbContext
    {
        public DbSet<TransactionHistory> TransactionHistory { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Privilege> Privileges { get; set; }

        public ETicketDataContext(DbContextOptions<ETicketDataContext> options) : base(options) { }

        public void EnsureCreated()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionHistory>()
            .HasOne<TicketType>(s => s.TicketType)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
           .HasOne(i => i.Role)
           .WithMany()
           .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
            .HasOne(i => i.Privilege)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
            .HasOne(i => i.Document)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);


            //'Introducing FOREIGN KEY constraint 'FK_Tickets_TransactionHistory_TransactionHistoryId' 
            //on table 'Tickets' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or 
            //ON UPDATE NO ACTION, or modify other FOREIGN KEY 
        }
    }
}