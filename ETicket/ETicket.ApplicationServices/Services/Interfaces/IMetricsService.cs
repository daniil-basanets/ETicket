using ETicket.ApplicationServices.Charts.DTOs;
using ETicket.ApplicationServices.Enums;
using System;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IMetricsService
    {
        ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, ChartScale chartScale = ChartScale.ByDays);
        ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, int routeId);
        ChartDto PassengersByRoutes(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId);
        ChartDto PassengersByPrivileges(DateTime startPeriod, DateTime endPeriod);
        ChartDto TicketsByTicketTypes(DateTime startPeriod, DateTime endPeriod);
    }
}
