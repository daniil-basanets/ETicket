using ETicket.ApplicationServices.DTOs.Charts;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ETicket.ApplicationServices.Enums;
using System.Text;

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

        public ChartDto PassengersByPrivileges(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutes)
        {
            if (startPeriod.CompareTo(endPeriod) == 1)
            {
                return new ChartDto() { ErrorMessage = EndLessStartError };
            }

            var data = uow.TicketVerifications.GetAll()
                .Where(d => d.VerificationUTCDate >= startPeriod && d.VerificationUTCDate <= endPeriod && d.IsVerified && 
                            (selectedRoutes.Length == 0 || selectedRoutes.Contains(d.Transport.RouteId)))
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


        public MultiLineChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId, ChartScale chartScale = ChartScale.ByDays)
        {
            MultiLineChartDto passengerTrafficChartData= new MultiLineChartDto();

            if (startPeriod.CompareTo(endPeriod) == 1)
            {
                passengerTrafficChartData.ErrorMessage = EndLessStartError;
            }

            var timeLabels = GetTimeLabels(startPeriod, endPeriod, chartScale);

            if (passengerTrafficChartData.ErrorMessage == null)
            {
                passengerTrafficChartData = GetMultiRoutePassengerTraffic(timeLabels, startPeriod, endPeriod, chartScale, selectedRoutesId);
            }

            return passengerTrafficChartData;
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

        private MultiLineChartDto GetMultiRoutePassengerTraffic(List<string> timeLabels, DateTime startPeriod, DateTime endPeriod, ChartScale chartScale, int[] selectedRoutesId)
        {
            var passengerInfo = uow.TicketVerifications.GetAll()
                   .Where(t => t.IsVerified && t.VerificationUTCDate.Date >= startPeriod.Date && t.VerificationUTCDate.Date <= endPeriod.Date
                        && (selectedRoutesId.Length == 0 || selectedRoutesId.Contains(t.Transport.RouteId)));

            var groupedTraffic = chartScale switch
            {
                ChartScale.ByDays => passengerInfo.GroupBy(d => new { d.Transport.Route.Number, Step = EF.Functions.DateDiffDay(startPeriod.Date, d.VerificationUTCDate.Date) }),
                ChartScale.ByMonths => passengerInfo.GroupBy(m => new { m.Transport.Route.Number, Step = EF.Functions.DateDiffMonth(startPeriod.Date, m.VerificationUTCDate.Date) }),
                ChartScale.ByYears => passengerInfo.GroupBy(y => new { y.Transport.Route.Number, Step = EF.Functions.DateDiffYear(startPeriod.Date, y.VerificationUTCDate.Date) }),
                _ => passengerInfo.GroupBy(d => new { d.Transport.Route.Number, Step = EF.Functions.DateDiffDay(startPeriod.Date, d.VerificationUTCDate.Date) })
            };

            var passengerTraffic = groupedTraffic.Select(g => new { g.Key.Number, g.Key.Step, PassengersCount = g.Count() })
                .OrderBy(t => t.Number)
                .ToList();

            var transportNumbers = passengerTraffic.Select(t => t.Number).Distinct().ToList();
            var chartData = new string[transportNumbers.Count, timeLabels.Count];

            for (int i = 0; i < transportNumbers.Count; i++)
            {
                for (int j = 0; j < timeLabels.Count; j++)
                {
                    chartData[i, j] = (passengerTraffic.Where(t => t.Number == transportNumbers[i] && t.Step == j).Count() != 0 ? passengerTraffic.Where(t => t.Number == transportNumbers[i] && t.Step == j).First().PassengersCount : 0).ToString();
                }
            }

            return new MultiLineChartDto()
            {
                Labels = timeLabels,
                Data = chartData,
                LineLable = transportNumbers
            };
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

        public ChartDto PassengersByDaysOfWeek(DateTime startPeriod, DateTime endPeriod)
        {
            //This array is needed to establish the order of days on the chart.
            //And to fill in the gaps, if there were no passengers for a certain day 
            var daysOfWeek = new DayOfWeek[] 
            { 
                DayOfWeek.Monday
                , DayOfWeek.Tuesday
                , DayOfWeek.Wednesday
                , DayOfWeek.Thursday
                , DayOfWeek.Friday
                , DayOfWeek.Saturday
                , DayOfWeek.Sunday          
            };

            StringBuilder errorMessageBuilder = new StringBuilder();

            if (startPeriod.CompareTo(endPeriod) == 1)
            {
                errorMessageBuilder.AppendLine(EndLessStartError);
            }
            if((endPeriod - startPeriod).Days < 7)
            {
                errorMessageBuilder.AppendLine("One week - minimum time period;");
            }

            var chartData = new List<string>();

            if (errorMessageBuilder.Length == 0)
            {
                DateTime nearestStartSunday = startPeriod;
                while(nearestStartSunday.DayOfWeek != DayOfWeek.Sunday)
                {
                    nearestStartSunday = nearestStartSunday.AddDays(-1);
                }
                    
                var passengerTraffic = uow.TicketVerifications.GetAll()
                    .Where(d => d.VerificationUTCDate.Date >= startPeriod.Date && d.VerificationUTCDate.Date <= endPeriod.Date && d.IsVerified)
                    .GroupBy(p => (DayOfWeek)(((int)EF.Functions.DateDiffDay((DateTime?)nearestStartSunday, (DateTime?)p.VerificationUTCDate)) % 7))
                    .OrderBy(g => g.Key)
                    .Select(g => new { DayOfWeek = g.Key, Count = g.Count() })
                    .ToDictionary(k => k.DayOfWeek, k => k.Count);

                chartData = daysOfWeek.Select(d => (passengerTraffic.ContainsKey(d) ? passengerTraffic[d] : 0).ToString()).ToList();
            }

            return new ChartDto()
            {
                Labels = daysOfWeek.Select(t => t.ToString()).ToList(),
                Data = chartData,
                ErrorMessage = errorMessageBuilder.ToString()
            };
        }

        public MultiLineChartDto PassengersByDaysOfWeek(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId)
        {
            //This array is needed to establish the order of days on the chart.
            //And to fill in the gaps, if there were no passengers for a certain day 
            var daysOfWeek = new DayOfWeek[]
            {
                DayOfWeek.Monday
                , DayOfWeek.Tuesday
                , DayOfWeek.Wednesday
                , DayOfWeek.Thursday
                , DayOfWeek.Friday
                , DayOfWeek.Saturday
                , DayOfWeek.Sunday
            };

            StringBuilder errorMessageBuilder = new StringBuilder();

            if (startPeriod.CompareTo(endPeriod) == 1)
            {
                errorMessageBuilder.AppendLine(EndLessStartError);
            }
            if ((endPeriod - startPeriod).Days < 7)
            {
                errorMessageBuilder.AppendLine("One week - minimum time period;");
            }

            var multiLineChartDto = new MultiLineChartDto();

            if (errorMessageBuilder.Length == 0)
            {
                DateTime nearestStartSunday = startPeriod;
                while (nearestStartSunday.DayOfWeek != DayOfWeek.Sunday)
                {
                    nearestStartSunday = nearestStartSunday.AddDays(-1);
                }

                var passengerTraffic = uow.TicketVerifications.GetAll()
                    .Where(d => d.VerificationUTCDate.Date >= startPeriod.Date && d.VerificationUTCDate.Date <= endPeriod.Date && d.IsVerified
                            && (selectedRoutesId.Length == 0 || selectedRoutesId.Contains(d.Transport.RouteId)))
                    .GroupBy(p => new { p.Transport.Route.Number, DayOfWeek = (((int)EF.Functions.DateDiffDay((DateTime?)nearestStartSunday, (DateTime?)p.VerificationUTCDate)) % 7) })
                    .Select(g => new { g.Key.Number, g.Key.DayOfWeek, PassengersCount = g.Count() })
                    .OrderBy(t => t.Number)
                    .ToList();

                var transportNumbers = passengerTraffic.Select(t => t.Number).Distinct().ToList();
                var chartData = new string[transportNumbers.Count, daysOfWeek.Length];

                for (int i = 0; i < transportNumbers.Count; i++)
                {
                    for (int j = 0; j < daysOfWeek.Length; j++)
                    {
                        chartData[i, j] = (passengerTraffic.Where(t => t.Number == transportNumbers[i] && t.DayOfWeek == (int)daysOfWeek[j]).Count() != 0 ? passengerTraffic.Where(t => t.Number == transportNumbers[i] && t.DayOfWeek == (int)daysOfWeek[j]).First().PassengersCount : 0).ToString();
                    }
                }

                multiLineChartDto.Labels = daysOfWeek.Select(t => t.ToString()).ToList();
                multiLineChartDto.Data = chartData;
                multiLineChartDto.LineLable = transportNumbers;
            }

            multiLineChartDto.ErrorMessage = errorMessageBuilder.ToString();

            return multiLineChartDto;
        }
    }
}
