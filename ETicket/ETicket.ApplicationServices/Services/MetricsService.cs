using ETicket.ApplicationServices.Charts.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.ApplicationServices.Services
{
    public class MetricsService : IMetricsService
    {
        private readonly IUnitOfWork uow;
        private readonly MapperService mapper;
        private readonly ITicketTypeService ticketTypeService;
        private readonly ITicketService ticketService;

        public MetricsService(IUnitOfWork uow, ITicketTypeService ticketTypeService, ITicketService ticketService)
        {
            this.uow = uow;
            this.ticketTypeService = ticketTypeService;
            this.ticketService = ticketService;
            mapper = new MapperService();
        }

        public ChartDto PassengersByPrivileges(DateTime start, DateTime end, int routeId)
        {
            throw new NotImplementedException();
        }

        public ChartDto PassengersByRoutes(DateTime start, DateTime end, int[] selectedRoutesId)
        {
            throw new NotImplementedException();
        }

        public ChartDto PassengersByTime(DateTime start, DateTime end, int routeId)
        {
            throw new NotImplementedException();
        }

        public ChartDto TicketsByTicketTypes(DateTime start, DateTime end)
        {
           var data = uow.Tickets.GetAll().GroupBy(t => t.TicketType.TypeName)
                   .Select(g => new { name = g.Key, count = g.Count().ToString() }).ToDictionary( k => k.name, k => k.count);

            ChartDto chartDto = new ChartDto();
            chartDto.Labels = data.Keys.ToArray();
            chartDto.Data = data.Values.ToArray();

            return chartDto;
        }
    }
}
