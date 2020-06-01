using ETicket.ApplicationServices.DTOs.Charts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IMetricsService
    {
        ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod);
        ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, int routeId);
        ChartDto PassengersByRoutes(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId);
        ChartDto PassengersByPrivileges(DateTime startPeriod, DateTime endPeriod);
        ChartDto TicketsByTicketTypes(DateTime startPeriod, DateTime endPeriod);
        ChartTableDto PassengersByHoursByRoutes(DateTime selectedDay, int[] selectedRoutes);
    }
}
