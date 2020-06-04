using ETicket.ApplicationServices.DTOs.Charts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IMetricsService
    {
        ChartDto PassengersByTime(DateTime startPeriod, DateTime endPeriod);
        ChartDto PassengersByPrivileges(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutes);
        ChartDto TicketsByTicketTypes(DateTime startPeriod, DateTime endPeriod);
        ChartTableDto PassengersByHoursByRoutes(DateTime selectedDay, int[] selectedRoutes);
    }
}
