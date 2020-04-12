﻿using DBContextLibrary.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DBContextLibrary.Domain
{
    public class ETicketDataContext : IdentityDbContext
    {
        // ToDo Region
        public DbSet<TransactionHistory> TransactionHistory { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> ETUsers { get; set; }
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
            //ToAsk - inquire Roman concerning Tabulations

            modelBuilder.Entity<TransactionHistory>()
                    .HasOne<TicketType>(s => s.TicketType)
                    .WithMany()
                    .OnDelete(DeleteBehavior.NoAction);

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