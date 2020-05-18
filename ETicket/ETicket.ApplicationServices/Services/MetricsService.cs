using ETicket.ApplicationServices.Charts.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ETicket.ApplicationServices.Services
{
    public class MetricsService : IMetricsService
    {
        #region Private members

        private readonly IUnitOfWork uow;
        private readonly MapperService mapper;
        private readonly ITicketTypeService ticketTypeService;
        private readonly ITicketService ticketService;
        private const int MaxDaysForChart = 366;

        #endregion

        public MetricsService(IUnitOfWork uow, ITicketTypeService ticketTypeService, ITicketService ticketService)
        {
            this.uow = uow;
            this.ticketTypeService = ticketTypeService;
            this.ticketService = ticketService;
            mapper = new MapperService();
        }

        public ChartDto PassengersByPrivileges(DateTime start, DateTime end)
        {
            if (start.CompareTo(end) == 1)
            {
                return new ChartDto() { ErrorMessage = "End date cannot be less than start date" };
            }

            var data = uow.TicketVerifications.GetAll()
                .Include(t => t.Ticket)
                .ThenInclude(u => u.User)
                .ThenInclude(p => p.Privilege)
                .Select(n => n.Ticket.User.Privilege)
                .GroupBy(p => p.Name)
                .Select(g => new { name = g.Key ?? "No privilege", count = g.Count().ToString() })
                .ToDictionary( k => k.name, k => k.count);


            var chartDto = new ChartDto {Labels = data.Keys.ToArray(), Data = data.Values.ToArray()};

            return chartDto;
        }

        public ChartDto PassengersByRoutes(DateTime start, DateTime end, int[] selectedRoutesId)
        {
            throw new NotImplementedException();
        }

        private IQueryable<int> GetPassengerForTimePeriod(DateTime start, DateTime end)
        {
            return uow.TicketVerifications.GetAll()
                .Where(t => (t.VerificationUTCDate > start && t.VerificationUTCDate <= end))
                .GroupBy(p => true)
                .Select(g => g.Count());
        }

        public ChartDto PassengersByTime(DateTime start, DateTime end, int routeId)
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

        public ChartDto PassengersByTime(DateTime start, DateTime end)
        {
            if (start.CompareTo(end) == 1)
            {
                return new ChartDto() { ErrorMessage = "End date cannot be less than start date" };
            }
            if ((end - start).TotalDays > MaxDaysForChart)
            {
                return new ChartDto() { ErrorMessage = "The period of time cannot be more than " + MaxDaysForChart };
            }

            List<DateTime> timePeriods = new List<DateTime>();
            
            for (DateTime d = start.Date.AddDays(-1); d <= end; d = d.AddDays(1))
            {
                timePeriods.Add(d);
            }

            var query = GetPassengerForTimePeriod(timePeriods[0], timePeriods[1]);
            
            for (int i = 1; i < timePeriods.Count - 1; i++)
            {
                query = query.Concat(GetPassengerForTimePeriod(timePeriods[i], timePeriods[i + 1]));
            }

            var data = query.ToList();

            return new ChartDto()
            {
                Labels = timePeriods.Skip(1).Select(d => d.ToShortDateString()).ToList(),
                Data = data.Select(d => d.ToString()).ToList()
            };
        }
    }
}
