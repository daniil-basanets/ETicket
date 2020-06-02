using ETicket.ApplicationServices.Charts.DTOs;
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
        private const string EndLessStartError = "End date cannot be less than start date";
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

        public ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, ChartScale chartScale)
        {
            string errorMessage = null;

            if (startPeriod.CompareTo(endPeriod) == 1)
            {
                errorMessage = EndLessStartError;
            }

            var timeLabels = new List<string>();

            if (chartScale == ChartScale.ByDays)
            {
                for (DateTime timePoint = startPeriod.Date; timePoint.Date < endPeriod.Date; timePoint = timePoint.AddDays((double)chartScale))
                {
                    timeLabels.Add(timePoint.ToShortDateString());
                }
                //Add the rest of the week, in case the interval is not divided by weeks
                timeLabels.Add(endPeriod.ToShortDateString());
            }
            if(chartScale == ChartScale.ByMonths)
            {
                for (DateTime timePoint = startPeriod.Date; timePoint.Month <= endPeriod.Month && timePoint.Year <= endPeriod.Year; timePoint = timePoint.AddMonths(1))
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

            var chartData = new Dictionary<int, int>();

            if (errorMessage == null)
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

                var trafficStatistics = groupedTraffic.OrderBy(d => d.Key)
                    .Select(g => new { Date = g.Key, Count = g.Count() })
                    .ToDictionary(k => k.Date, k => k.Count);

                for (int i = 0; i < timeLabels.Count; i++)
                {
                    chartData.Add(i, trafficStatistics.ContainsKey(i) ? trafficStatistics[i] : 0);
                }
            }

            return new ChartDto()
            {
                Labels = timeLabels,
                Data = chartData.Values.Select(d => d.ToString()).ToList(),
                ErrorMessage = errorMessage
            };
        }
    }
}
