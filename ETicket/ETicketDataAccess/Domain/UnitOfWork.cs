using ETicket.DataAccess.Domain.Interfaces;
using ETicket.DataAccess.Domain.Repositories;
using System;

namespace ETicket.DataAccess.Domain
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region private members

        private ETicketDataContext eTicketDataContext;

        private DocumentRepository documentRepository;
        private DocumentTypeRepository documentTypeRepository;
        private PrivilegeRepository privilegeRepository;
        private TicketRepository ticketRepository;
        private TicketTypeRepository ticketTypeRepository;
        private TransactionHistoryRepository transactionHistoryRepository;
        private UserRepository userRepository;
        private RouteStationRepository routeStationRepository;

        #endregion

        public DocumentRepository Documents
        {
            get
            {
                if (documentRepository == null)
                    documentRepository = new DocumentRepository(eTicketDataContext);
                return documentRepository;
            }
        }

        public DocumentTypeRepository DocumentTypes
        {
            get
            {
                if (documentTypeRepository == null)
                    documentTypeRepository = new DocumentTypeRepository(eTicketDataContext);
                return documentTypeRepository;
            }
        }

        public PrivilegeRepository Privileges
        {
            get
            {
                if (privilegeRepository == null)
                    privilegeRepository = new PrivilegeRepository(eTicketDataContext);
                return privilegeRepository;
            }
        }

        public TicketRepository Tickets
        {
            get
            {
                if (ticketRepository == null)
                    ticketRepository = new TicketRepository(eTicketDataContext);
                return ticketRepository;
            }
        }

        public TicketTypeRepository TicketTypes
        {
            get
            {
                if (ticketTypeRepository == null)
                    ticketTypeRepository = new TicketTypeRepository(eTicketDataContext);

                return ticketTypeRepository;
            }
        }

        public TransactionHistoryRepository TransactionHistory
        {
            get
            {
                if (transactionHistoryRepository == null)
                    transactionHistoryRepository = new TransactionHistoryRepository(eTicketDataContext);

                return transactionHistoryRepository;
            }
        }

        public UserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(eTicketDataContext);
                return userRepository;
            }
        }

        public RouteStationRepository RouteStation
        {
            get
            {
                if (routeStationRepository == null)
                {
                    routeStationRepository = new RouteStationRepository(eTicketDataContext);
                }

                return routeStationRepository;
            }
        }

        public void Save()
        {
            eTicketDataContext.SaveChanges();
        }

        public UnitOfWork(ETicketDataContext eTicketDataContext)
        {
            this.eTicketDataContext = eTicketDataContext;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    eTicketDataContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
