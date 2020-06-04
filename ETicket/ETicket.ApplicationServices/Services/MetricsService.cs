using ETicket.ApplicationServices.DTOs.Charts;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ETicket.ApplicationServices.Enums;

namespace ETicket.ApplicationServices.Services
{
    public class MetricsService : IMetricsService
    {
        #region Private members

        private readonly IUnitOfWork uow;
        private const int HoursInDay = 24;
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
                .Where(d => d.VerificationUTCDate >= startPeriod && d.VerificationUTCDate <= endPeriod && d.IsVerified)
                .Include(t => t.Ticket)
                .ThenInclude(u => u.User)
                .ThenInclude(p => p.Privilege)
                .Select(n => n.Ticket.User.Privilege)
                .GroupBy(p => p.Name)
                .Select(g => new { name = g.Key ?? "No privilege", count = g.Count().ToString() })
                .ToDictionary(k => k.name, k => k.count);


            var chartDto = new ChartDto { Labels = data.Keys.ToArray(), Data = data.Values.ToArray() };

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

        public ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, ChartScale chartScale)
        {
            string errorMessage = null;

            if (startPeriod.CompareTo(endPeriod) == 1)
            {
                errorMessage = EndLessStartError;
            }

            var timeLabels = GetTimeLabels(startPeriod, endPeriod, chartScale);

            var chartData = new Dictionary<int, int>();

            if (errorMessage == null)
            {
                var passengerTraffic = GetPassengerTraffic(startPeriod, endPeriod, chartScale);

                for (int i = 0; i < timeLabels.Count; i++)
                {
                    chartData.Add(i, passengerTraffic.ContainsKey(i) ? passengerTraffic[i] : 0);
                }
            }

            return new ChartDto()
            {
                Labels = timeLabels,
                Data = chartData.Values.Select(d => d.ToString()).ToList(),
                ErrorMessage = errorMessage
            };
        }

        private List<string> GetTimeLabels(DateTime startPeriod, DateTime endPeriod, ChartScale chartScale)
        {
            var timeLabels = new List<string>();

            if (chartScale == ChartScale.ByDays)
            {
                for (DateTime timePoint = startPeriod.Date; timePoint.Date <= endPeriod.Date; timePoint = timePoint.AddDays(1))
                {
                    timeLabels.Add(timePoint.ToShortDateString());
                }
            }
            if (chartScale == ChartScale.ByMonths)
            {
                for (DateTime timePoint = startPeriod.Date; timePoint.Month <= endPeriod.Month || timePoint.Year < endPeriod.Year; timePoint = timePoint.AddMonths(1))
                {
                    timeLabels.Add(timePoint.ToString("MMMM, yyyy"));
                }
            }
            if (chartScale == ChartScale.ByYears)
            {
                for (DateTime timePoint = startPeriod.Date; timePoint.Year <= endPeriod.Year; timePoint = timePoint.AddYears(1))
                {
                    timeLabels.Add(timePoint.Year.ToString());
                }
            }

            return timeLabels;
        }

        private Dictionary<int, int> GetPassengerTraffic(DateTime startPeriod, DateTime endPeriod, ChartScale chartScale)
        {
            var passengerTraffic = uow.TicketVerifications.GetAll()
                   .Where(t => t.IsVerified && t.VerificationUTCDate.Date >= startPeriod.Date && t.VerificationUTCDate.Date <= endPeriod.Date);

            var groupedTraffic = chartScale switch
            {
                ChartScale.ByDays => passengerTraffic.GroupBy(d => EF.Functions.DateDiffDay(startPeriod.Date, d.VerificationUTCDate.Date)),
                ChartScale.ByMonths => passengerTraffic.GroupBy(m => EF.Functions.DateDiffMonth(startPeriod.Date, m.VerificationUTCDate.Date)),
                ChartScale.ByYears => passengerTraffic.GroupBy(y => EF.Functions.DateDiffYear(startPeriod.Date, y.VerificationUTCDate.Date)),
                _ => passengerTraffic.GroupBy(y => EF.Functions.DateDiffDay(startPeriod.Date, y.VerificationUTCDate.Date)),
            };

            return groupedTraffic.OrderBy(d => d.Key)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToDictionary(k => k.Date, k => k.Count); ;
        }

        public ChartTableDto PassengersByHoursByRoutes(DateTime selectedDay, int[] selectedRoutesId)
        {
            var chartData = uow.TicketVerifications.GetAll()
                               .Include(t => t.Transport)
                               .ThenInclude(r => r.Route)
                               .Where(t => t.IsVerified && t.VerificationUTCDate.Date == selectedDay.Date
                                                        && (selectedRoutesId.Length == 0 || selectedRoutesId.Contains(t.Transport.RouteId)))
                               .GroupBy(x => new { x.Transport.Route.Number, x.VerificationUTCDate.Hour })
                               .Select(g => new { g.Key.Number, g.Key.Hour, PassengersCount = g.Count() })
                               .OrderBy(t => t.Number)
                               .ToList();

            var maxPassengers = chartData.Max(m => m.PassengersCount);
            var routesCount = chartData.GroupBy(t => t.Number).Count();

            ChartTableDto chartTableDto = new ChartTableDto
            {
                MaxPassengersByRoute = maxPassengers,
                Data = new string[HoursInDay, routesCount]
            };

            var labels = chartData.Select(t => t.Number).Distinct();
            chartTableDto.Labels = labels.ToList();

            for (int i = 0; i < HoursInDay; i++)
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
