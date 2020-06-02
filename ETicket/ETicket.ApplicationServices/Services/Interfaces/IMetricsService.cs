using System;
using System.Collections.Generic;
using System.Text;
using ETicket.ApplicationServices.DTOs.Charts;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IMetricsService
    {
        ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod);
        ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod, int routeId);
        ChartDto PassengersByRoutes(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId);
        ChartByRoutesDto PassengersByPrivileges(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId);
        ChartDto TicketsByTicketTypes(DateTime startPeriod, DateTime endPeriod);
    }
}
