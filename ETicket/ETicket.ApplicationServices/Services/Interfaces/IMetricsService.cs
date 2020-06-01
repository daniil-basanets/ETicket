using ETicket.ApplicationServices.Charts.DTOs;
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
        ChartDto PassengersByPrivilegesByRoute(DateTime startPeriod, DateTime endPeriod, int routeId);
        ChartDto TicketsByTicketTypes(DateTime startPeriod, DateTime endPeriod);
    }
}
