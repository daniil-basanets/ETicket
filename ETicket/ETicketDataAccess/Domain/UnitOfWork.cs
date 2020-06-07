using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.DataAccess.Domain.Repositories;
using System;

namespace ETicket.DataAccess.Domain
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region private members

        private readonly ETicketDataContext eTicketDataContext;

        private IRepository<Document,Guid> documentRepository;
        private IRepository<DocumentType,int> documentTypeRepository;
        private IRepository<Privilege,int> privilegeRepository;
        private IRepository<Ticket,Guid> ticketRepository;
        private IRepository<TicketType,int> ticketTypeRepository;
        private IRepository<TicketVerification,Guid> ticketVerificationRepository;
        private IRepository<TransactionHistory,Guid> transactionHistoryRepository;
        private IRepository<User,Guid> userRepository;
        private IRepository<Carrier,int> carrierRepository;
        private IRepository<RouteStation,int> routeStationRepository;
        private SecretCodeRepository secretCodeRepository;
        private IRepository<Route,int> routeRepository;
        private IRepository<Station,int> stationRepository;
        private IRepository<Transport,int> transportRepository;
        private IRepository<PriceList,int> priceListRepository;
        private IRepository<Area,int> areaRepository;
        private IRepository<TicketArea,TicketArea> ticketAreaRepository;

        #endregion

        public IRepository<Document,Guid> Documents
        {
            get
            {
                if (documentRepository == null)
                    documentRepository = new DocumentRepository(eTicketDataContext);
                return documentRepository;
            }
        }

        public IRepository<Route,int> Routes
        {
            get
            {
                if (routeRepository == null)
                    routeRepository = new RouteRepository(eTicketDataContext);
                return routeRepository;
            }
        }

        public IRepository<DocumentType,int> DocumentTypes
        {
            get
            {
                if (documentTypeRepository == null)
                    documentTypeRepository = new DocumentTypeRepository(eTicketDataContext);
                return documentTypeRepository;
            }
        }

        public IRepository<Privilege,int> Privileges
        {
            get
            {
                if (privilegeRepository == null)
                    privilegeRepository = new PrivilegeRepository(eTicketDataContext);
                return privilegeRepository;
            }
        }

        public IRepository<Ticket,Guid> Tickets
        {
            get
            {
                if (ticketRepository == null)
                    ticketRepository = new TicketRepository(eTicketDataContext);
                return ticketRepository;
            }
        }

        public IRepository<TicketType,int> TicketTypes
        {
            get
            {
                if (ticketTypeRepository == null)
                    ticketTypeRepository = new TicketTypeRepository(eTicketDataContext);

                return ticketTypeRepository;
            }
        }

        public IRepository<TicketVerification,Guid> TicketVerifications
        {
            get
            {
                if (ticketVerificationRepository == null)
                    ticketVerificationRepository = new TicketVerificationRepository(eTicketDataContext);

                return ticketVerificationRepository;
            }
        }

        public IRepository<TransactionHistory,Guid> TransactionHistory
        {
            get
            {
                if (transactionHistoryRepository == null)
                    transactionHistoryRepository = new TransactionHistoryRepository(eTicketDataContext);

                return transactionHistoryRepository;
            }
        }

        public IRepository<User,Guid> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(eTicketDataContext);
                return userRepository;
            }
        }
        
        public IRepository<PriceList,int> PriceList
        {
            get
            {
                if (priceListRepository == null)
                    priceListRepository = new PriceListRepository(eTicketDataContext);

                return priceListRepository;
            }
        }

        public IRepository<Carrier,int> Carriers
        {
            get
            {
                if(carrierRepository == null)
                {
                    carrierRepository = new CarrierRepository(eTicketDataContext);
                }
                return carrierRepository;
            }
        }
        public IRepository<RouteStation,int> RouteStation
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

        public IRepository<TicketArea, TicketArea> TicketArea
        {
            get
            {
                if (ticketAreaRepository == null)
                {
                    ticketAreaRepository = new TicketAreaRepository(eTicketDataContext);
                }

                return ticketAreaRepository;
            }
        }

        public ISecretCodeRepository SecretCodes
        {
            get
            {
                if (secretCodeRepository == null)
                    secretCodeRepository = new SecretCodeRepository(eTicketDataContext);

                return secretCodeRepository;
            }
        }

        public IRepository<Station,int> Stations
        {
            get
            {
                if (stationRepository == null)
                    stationRepository = new StationRepository(eTicketDataContext);

                return stationRepository;
            }
        }
        
        public IRepository<Area,int> Areas
        {
            get 
            {
                if (areaRepository == null)
                    areaRepository = new AreaRepository(eTicketDataContext);

                return areaRepository;
            }
        }

        public IRepository<Transport,int> Transports
        {
            get
            {
                if (transportRepository == null)
                    transportRepository = new TransportRepository(eTicketDataContext);

                return transportRepository;
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
