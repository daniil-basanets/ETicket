using ETicket.ApplicationServices.DTOs.Charts;
using ETicket.ApplicationServices.Enums;
using System;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IMetricsService
    {
        ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, ChartScale chartScale = ChartScale.ByDays);
        MultiLineChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId, ChartScale chartScale = ChartScale.ByDays);
        ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, int routeId);
        ChartDto PassengersByRoutes(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId);
        ChartDto PassengersByPrivileges(DateTime startPeriod, DateTime endPeriod);
        ChartDto TicketsByTicketTypes(DateTime startPeriod, DateTime endPeriod);
        ChartDto PassengersByDaysOfWeek(DateTime startPeriod, DateTime endPeriod);
        ChartTableDto PassengersByHoursByRoutes(DateTime selectedDay, int[] selectedRoutes);
    }
}
