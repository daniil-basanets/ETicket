using ETicket.ApplicationServices.DTOs.Charts;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ETicket.ApplicationServices.Services
{
    public class MetricsService : IMetricsService
    {
        #region Private members

        private readonly IUnitOfWork uow;
        private const int MaxDaysForChart = 366;
        private const string EndLessStartError = "End date cannot be less than start date";
        private const string MaxDaysForChartError = "The period of time cannot be more than {0} days";

        #endregion

        public MetricsService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public ChartDto PassengersByPrivileges(DateTime startPeriod, DateTime endPeriod)
        {
            if (startPeriod.CompareTo(endPeriod) == 1)
            {
                return new ChartDto() { ErrorMessage = EndLessStartError };
            }

            var data = uow.TicketVerifications.GetAll()
                .Where(d=>d.VerificationUTCDate >= startPeriod && d.VerificationUTCDate <= endPeriod && d.IsVerified)
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

        public ChartDto PassengersByRoutes(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId)
        {
            throw new NotImplementedException();
        }

        public ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, int routeId)
        {
            throw new NotImplementedException();
        }

        public ChartDto TicketsByTicketTypes(DateTime startPeriod, DateTime endPeriod)
        {
            if (startPeriod.CompareTo(endPeriod) == 1)
            {
                return new ChartDto() { ErrorMessage = EndLessStartError };
            }

            var data = uow.Tickets.GetAll()
                 .Where(t => t.CreatedUTCDate >= startPeriod && t.CreatedUTCDate <= endPeriod)
                 .GroupBy(t => t.TicketType.TypeName)
                 .Select(g => new { name = g.Key, count = g.Count().ToString() })
                 .ToDictionary(k => k.name, k => k.count);

            ChartDto chartDto = new ChartDto();
            chartDto.Labels = data.Keys.ToArray();
            chartDto.Data = data.Values.ToArray();

            return chartDto;
        }

        public ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod)
        {
            if (startPeriod.CompareTo(endPeriod) == 1)
            {
                return new ChartDto() { ErrorMessage = EndLessStartError };
            }
            if ((endPeriod - startPeriod).TotalDays > MaxDaysForChart)
            {
                return new ChartDto() { ErrorMessage = String.Format(MaxDaysForChartError, MaxDaysForChart) };
            }

            List<DateTime> timePeriods = new List<DateTime>();

            for (DateTime day = startPeriod.Date; day <= endPeriod; day = day.AddDays(1))
            {
                timePeriods.Add(day);
            }

            var chartData = uow.TicketVerifications.GetAll()
                .Where(t => t.IsVerified && t.VerificationUTCDate.Date >= startPeriod.Date && t.VerificationUTCDate.Date <= endPeriod.Date)
                .GroupBy(d => d.VerificationUTCDate.Date)
                .OrderBy(d => d.Key)
                .Select(g => new { date = g.Key, count = g.Count()})
                .ToDictionary(k => k.date, k => k.count);

            return new ChartDto()
            {
                Labels = timePeriods.Select(d => d.ToShortDateString()).ToList(),
                Data = timePeriods.Select(d => (chartData.ContainsKey(d) ? chartData[d] : 0).ToString()).ToList()
            };
        }

        public ChartTableDto PassengersByHoursByRoutes(DateTime selectedDay, int [] selectedRoutesId)
        {
            var chartData = uow.TicketVerifications.GetAll()
                               .Include(t => t.Transport)
                               .ThenInclude(r => r.Route)
                               .Where(t => t.IsVerified && t.VerificationUTCDate.Date == selectedDay.Date 
                                                        && (selectedRoutesId.Length == 0 || selectedRoutesId.Contains(t.Transport.RouteId)))
                               .GroupBy(x => new { x.Transport.Route.Number, x.VerificationUTCDate.Hour })
                               .Select(g => new { g.Key.Number, g.Key.Hour, PassengersCount = g.Count()}).ToList();

            ChartTableDto chartTableDto = new ChartTableDto();
            var routesCount = chartData.GroupBy(t => t.Number).Count();
            var maxPassengers = chartData.Max(m => m.PassengersCount);
            chartTableDto.MaxPassengersByRoute = maxPassengers;
            chartTableDto.Data = new string[24, routesCount];

            var labels = chartData.Select(t => t.Number).Distinct();
            chartTableDto.Labels = labels.ToList();

            for (int i = 0; i < 24; i++)
            {
                var temp = chartData.Where(t => t.Hour == i);

                foreach (var item in temp)
                {
                    var j = chartTableDto.Labels.IndexOf(item.Number);
                    chartTableDto.Data[i, j] = item.PassengersCount.ToString();
                }                
            }

            return chartTableDto;
        }
    }
}
