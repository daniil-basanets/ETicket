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
        public DbSet<TicketVerification> TicketVerifications { get; set; }
        public DbSet<User> ETUsers { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<Carrier> Carriers { get; set; }
        public DbSet<RouteStation> RouteStations { get; set; }
        public DbSet<TicketArea> TicketAreas { get; set; }
        public DbSet<SecretCode> SecretCodes { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Transport> Transports { get; set; }
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

            modelBuilder.Entity<User>()
                   .HasOne(i => i.Privilege)
                   .WithMany()
                   .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                    .HasOne(i => i.Document)
                    .WithMany()
                    .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<RouteStation>()
                    .HasKey(rt => new { rt.RouteId, rt.StationId });

            modelBuilder.Entity<RouteStation>()
                    .HasOne(r => r.Route)
                    .WithMany(r => r.RouteStations)
                    .HasForeignKey(rt => rt.RouteId);

            modelBuilder.Entity<RouteStation>()
                   .HasOne(r => r.Station)
                   .WithMany(r => r.RouteStations)
                   .HasForeignKey(rt => rt.StationId);

            modelBuilder.Entity<Route>()
                    .HasOne<Station>(s => s.LastStation)
                    .WithMany()
                    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Route>()
                    .HasOne<Station>(s => s.FirstStation)
                    .WithMany()
                    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Ticket>()
                    .HasOne<TicketType>(s => s.TicketType)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Document>()
                    .HasOne<DocumentType>(s => s.DocumentType)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketArea>()
                    .HasKey(ta => new { ta.TicketId, ta.AreaId });

            modelBuilder.Entity<TicketArea>()
                    .HasOne(r => r.Area)
                    .WithMany(r => r.TicketArea)
                    .HasForeignKey(rt => rt.AreaId);

            modelBuilder.Entity<TicketArea>()
                   .HasOne(r => r.Ticket)
                   .WithMany(r => r.TicketArea)
                   .HasForeignKey(rt => rt.TicketId);

            base.OnModelCreating(modelBuilder);
        }
    }
}