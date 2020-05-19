using ETicket.DataAccess.Domain.Repositories;

namespace ETicket.DataAccess.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        DocumentRepository Documents { get; }

        RouteRepository Routes { get; }

        RouteStationRepository RouteStation { get; }

        DocumentTypeRepository DocumentTypes { get; }

        PrivilegeRepository Privileges { get; }

        TicketRepository Tickets { get; }

        TicketTypeRepository TicketTypes { get; }

        TicketAreaRepository TicketArea { get; }

        TicketVerificationRepository TicketVerifications { get; }
        
        TransactionHistoryRepository TransactionHistory { get; }

        UserRepository Users { get; }

        CarrierRepository Carriers { get; }

        SecretCodeRepository SecretCodes { get; }

        AreaRepository Areas { get; }

        StationRepository Stations { get; }
        
        TransportRepository Transports { get; }
        
        PriceListRepository PriceList { get; }

        void Save();
    }
}
