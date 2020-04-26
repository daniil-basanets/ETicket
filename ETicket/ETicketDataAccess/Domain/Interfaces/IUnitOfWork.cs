using ETicket.DataAccess.Domain.Repositories;

namespace ETicket.DataAccess.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        DocumentRepository Documents { get; }

        DocumentTypeRepository DocumentTypes { get; }

        PrivilegeRepository Privileges { get; }

        TicketRepository Tickets { get; }

        TicketTypeRepository TicketTypes { get; }

        TransactionHistoryRepository TransactionHistory { get; }

        UserRepository Users { get; }

        CarrierRepository Carriers { get; }
        
        AreaRepository Areas { get; }

        PriceListRepository PriceList { get; }

        RouteRepository Routes { get; }

        void Save();
    }
}
