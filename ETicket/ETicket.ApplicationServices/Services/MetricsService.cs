using ETicket.ApplicationServices.Charts.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
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

        public IQueryable<string> GetATimeQuert(DateTime start, DateTime end)
        {
            return uow.TicketVerifications.GetAll()
                .Where(t => (t.VerificationUTCDate > start && t.VerificationUTCDate <= end))
                .GroupBy(p => true)
                .Select(g => g.Count().ToString());
        }

        public ChartDto PassengersByTime(DateTime start, DateTime end, int accuracy, int routeId)
        {
            throw new NotImplementedException();
        }

        public ChartDto TicketsByTicketTypes(DateTime start, DateTime end)
        {
           var data = uow.Tickets.GetAll().GroupBy(t => t.TicketType.TypeName)
                   .Select(g => new { name = g.Key, count = g.Count().ToString() })
                   .ToDictionary( k => k.name, k => k.count);

            ChartDto chartDto = new ChartDto();
            chartDto.Labels = data.Keys.ToArray();
            chartDto.Data = data.Values.ToArray();

            return chartDto;
        }

        public ChartDto PassengersByTime(DateTime start, DateTime end, int accuracy)
        {
            if (start.CompareTo(end) == 1)
            {
                throw new InvalidOperationException();
            }

            var timeSpan = end - start;
            var minutes = timeSpan.TotalHours / accuracy;
            List<DateTime> timePeriods = new List<DateTime>();
            for (int i = 0; i <= accuracy; i++)
            {
                timePeriods.Add(start);
                start = start.AddHours(minutes);
            }

            var query = GetATimeQuert(timePeriods[0], timePeriods[1]);
            for (int i = 1; i < timePeriods.Count - 1; i++)
            {
                query = query.Concat(GetATimeQuert(timePeriods[i], timePeriods[i + 1]));
            }

            var data = query.ToList();

            ChartDto chartDto = new ChartDto();
            chartDto.Labels = timePeriods.Skip(1).Select(d => d.ToShortDateString()).ToList();
            chartDto.Data = data;

            return chartDto;
        }
    }
}
