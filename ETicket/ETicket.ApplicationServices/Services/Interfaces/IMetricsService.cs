using ETicket.ApplicationServices.Charts.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using static ETicket.ApplicationServices.Services.MetricsService;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public enum ChartScale
    {
        ByDays = 1,
        ByWeeks = 7,
        ByMonths = 30
    }

    public interface IMetricsService
    {
        ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, ChartScale chartScale = ChartScale.ByDays);
        ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, int routeId);
        ChartDto PassengersByRoutes(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId);
        ChartDto PassengersByPrivileges(DateTime startPeriod, DateTime endPeriod);
        ChartDto TicketsByTicketTypes(DateTime startPeriod, DateTime endPeriod);
    }
}
