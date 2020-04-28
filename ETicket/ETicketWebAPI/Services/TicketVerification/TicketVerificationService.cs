using ETicket.DataAccess.Domain.Interfaces;
using ETicket.WebAPI.Models.TicketVerification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.WebAPI.Services.TicketVerification
{
    public class TicketVerificationService
    {
        private IUnitOfWork unitOfWork;

        public TicketVerificationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;           
        }

        //public bool VerifyTicket(VerifyTicketRequest request)
        //{
        //    var ticket = unitOfWork.Tickets.Get(request.TicketId);
        //    var transport = unitOfWork.Transports.Get(request.TransportId);
        //    var station = unitOfWork

        //    return false;
        //}
    }
}
