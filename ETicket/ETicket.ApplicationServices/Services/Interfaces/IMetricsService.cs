using ETicket.ApplicationServices.DTOs.Charts;
using ETicket.ApplicationServices.Enums;
using System;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IMetricsService
    {
        ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, ChartScale chartScale = ChartScale.ByDays);
        MultiLineChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId, ChartScale chartScale = ChartScale.ByDays);
        ChartDto PassengersByPrivileges(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutes);
        ChartDto TicketsByTicketTypes(DateTime startPeriod, DateTime endPeriod);
        ChartDto PassengersByDaysOfWeek(DateTime startPeriod, DateTime endPeriod);
        MultiLineChartDto PassengersByDaysOfWeek(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId);
        ChartTableDto PassengersByHoursByRoutes(DateTime selectedDay, int[] selectedRoutes);
    }
}
