using ETicket.ApplicationServices.Charts.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.Services
{
    public class MetricsService : IMetricsService
    {
        public ChartDto PassengersByPrivileges(DateTime start, DateTime end, int routeId)
        {
            throw new NotImplementedException();
        }

        public ChartDto PassengersByRoutes(DateTime start, DateTime end, int[] selectedRoutesId)
        {
            throw new NotImplementedException();
        }

        public ChartDto PassengersByTime(DateTime start, DateTime end, int routeId)
        {
            throw new NotImplementedException();
        }

        public ChartDto TicketsByTicketTypes(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
    }
}
