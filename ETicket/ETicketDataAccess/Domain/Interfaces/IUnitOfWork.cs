using ETicketDataAccess.Domain.Repositories;

namespace ETicketDataAccess.Domain.Interfaces
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

        void Save();
    }
}
