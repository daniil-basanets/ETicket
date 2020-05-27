using ETicket.ApplicationServices.Charts.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IMetricsService
    {
        ChartDto PassengersByTime(DateTime start, DateTime end);
        ChartDto PassengersByTime(DateTime start, DateTime end, int routeId);
        ChartDto PassengersByRoutes(DateTime start, DateTime end, int[] selectedRoutesId);
        ChartDto PassengersByPrivileges(DateTime start, DateTime end);
        ChartDto TicketsByTicketTypes(DateTime start, DateTime end);
        ChartDto PassengersByDaysOfWeek(DateTime startPeriod, DateTime endPeriod);
    }
}
