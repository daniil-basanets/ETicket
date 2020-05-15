using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.ApplicationServices.Services
{
    public class TicketVerificationService : ITicketVerificationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITicketService ticketService;
        private readonly MapperService mapper;

        public TicketVerificationService(IUnitOfWork unitOfWork, ITicketService ticketService)
        {
            this.unitOfWork = unitOfWork;
            this.ticketService = ticketService;
            mapper = new MapperService();
        }

        public IEnumerable<TicketVerification> GetTicketVerifications()
        {
            return unitOfWork.TicketVerifications
                    .GetAll()
                    .Include(x => x.Station)
                    .Include(x => x.Ticket)
                    .Include(x => x.Transport);
        }

        public TicketVerificationDto GetTicketVerificationById(Guid id)
        {
            var ticketVerification = unitOfWork.TicketVerifications.Get(id);

            return mapper.Map<TicketVerification, TicketVerificationDto>(ticketVerification);
        }

        public IEnumerable<TicketVerification> GetVerificationHistoryByTicketId(Guid ticketId)
        {
            var history = unitOfWork.TicketVerifications
                    .GetAll()
                    .Where(t => t.TicketId == ticketId)
                    .OrderByDescending(t => t.VerificationUTCDate);

            return history.ToList();
        }


        public void Create(TicketVerificationDto ticketVerificationDto)
        {
            var ticketVerification = mapper.Map<TicketVerificationDto, TicketVerification>(ticketVerificationDto);

            unitOfWork.TicketVerifications.Create(ticketVerification);
            unitOfWork.Save();
        }

        public VerifyTicketResponceDto VerifyTicket(Guid ticketId, long transportId, float longitude, float latitude)
        {
            var ticket = unitOfWork
                    .Tickets
                    .GetAll()
                    .Include(t => t.TicketArea)
                    .FirstOrDefault(t => t.Id == ticketId);

            if (ticket == null)
            {
                return new VerifyTicketResponceDto { IsValid = false, ErrorMessage = "Ticket was not found" };
            }
            if (ticket.ActivatedUTCDate == null)
            {
                ticketService.Activate(ticket.Id);
            }

            var transport = unitOfWork.Transports.Get(transportId);

            if (transport == null)
            {
                return new VerifyTicketResponceDto { IsValid = false, ErrorMessage = "Transport was not found" };
            }

            var station = GetNearestStationOnRoute(
                    transport.RouteId, longitude, latitude);

            if (station == null)
            {
                return new VerifyTicketResponceDto { IsValid = false, ErrorMessage = "Station was not found" };
            }

            VerifyTicketResponceDto result = new VerifyTicketResponceDto { IsValid = true};

            //Check for expired ticket
            if (ticket.ExpirationUTCDate < DateTime.UtcNow)
            {
                result = new VerifyTicketResponceDto { IsValid = false, ErrorMessage = "Ticket expired" };
            }
            //Compare ticket areas with train station
            if (!ticket.TicketArea.Any(t => t.AreaId == station.Area.Id))
            {
                result = new VerifyTicketResponceDto { IsValid = false, ErrorMessage = "Ticket does not contain the zone" };
            }

            var ticketVerificationDto = new TicketVerificationDto
            {
                Id = Guid.NewGuid(),
                VerificationUTCDate = DateTime.UtcNow,
                TicketId = ticketId,
                TransportId = transport.Id,
                StationId = station.Id,
                IsVerified = result.IsValid
            };
            Create(ticketVerificationDto);

            return result;
        }

        private void ActivateTicket(Ticket ticket)
        {
            ticket.ActivatedUTCDate = DateTime.UtcNow;
            ticket.ExpirationUTCDate = DateTime.UtcNow.AddHours(ticket.TicketType.DurationHours);
            unitOfWork.Tickets.Update(ticket);
            unitOfWork.Save();
        }

        private Station GetNearestStationOnRoute(int routeId, float latitude, float longitude)
        {
            return unitOfWork
                    .RouteStation
                    .GetAll()
                    .Where(t => t.Route.Id == routeId)
                    .Include(t => t.Station.Area)
                    .Select(t => t.Station)
                    .OrderBy(t => Math.Sqrt(Math.Pow(t.Latitude * latitude, 2) + Math.Pow(t.Longitude * longitude, 2)))
                    .FirstOrDefault();
        }
    }
}
