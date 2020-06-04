using System;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Repositories;

namespace ETicket.DataAccess.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Document,Guid> Documents { get; }

        IRepository<Route,int> Routes { get; }
        
        IRepository<DocumentType,int> DocumentTypes { get; }
        
        IRepository<RouteStation, int> RouteStation { get; }

        IRepository<Privilege,int> Privileges { get; }

        IRepository<Ticket,Guid> Tickets { get; }

        IRepository<TicketType,int> TicketTypes { get; }

        IRepository<TicketArea, TicketArea> TicketArea { get; }

        IRepository<TicketVerification,Guid> TicketVerifications { get; }
        
        IRepository<TransactionHistory,Guid> TransactionHistory { get; }

        IRepository<User,Guid> Users { get; }

        IRepository<Carrier,int> Carriers { get; }

        ISecretCodeRepository SecretCodes { get; }
        
        IRepository<Area,int> Areas { get; }
        
        IRepository<Station,int> Stations { get; }
        
        IRepository<Transport,int> Transports { get; }
        
        IRepository<PriceList,int> PriceList { get; }

        void Save();
    }
}
