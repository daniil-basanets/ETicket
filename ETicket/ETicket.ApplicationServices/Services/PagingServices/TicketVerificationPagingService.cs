using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ETicket.ApplicationServices.Extensions;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.ApplicationServices.Services.PagingServices
{
    public class TicketVerificationPagingService : IDataTablePagingService<TicketVerification>
    {
        private readonly IUnitOfWork unitOfWork;

        public TicketVerificationPagingService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<TicketVerification> GetAll()
        {
            return unitOfWork.TicketVerifications
                    .GetAll()
                    .Include(x => x.Station)
                    .Include(x => x.Ticket)
                    .Include(x => x.Transport);
        }

        public Expression<Func<TicketVerification, bool>> GetSingleFilterExpression(string columnName, string filterValue)
        {
            return columnName switch
            {
                "ticket" => (t => t.Ticket.Id.ToString().StartsWith(filterValue)),
                "verificationUTCDate" => (t => t.VerificationUTCDate.Date == filterValue.ParseToDate()),
                "station" => (t => t.Station.Name.StartsWith(filterValue)),
                "transport" => (t => t.Transport.VehicleNumber.StartsWith(filterValue)),
                "isVerified" => (t => t.IsVerified == filterValue.ParseToBoolean()),
                _ => (t => true)
            };
        }

        public IList<Expression<Func<TicketVerification, bool>>> GetFilterExpressions(string[] columnNames, string[] filterValues)
        {
            var result = new List<Expression<Func<TicketVerification, bool>>>();

            for (int i = 0; i < columnNames.Length; i++)
            {
                result.Add(GetSingleFilterExpression(columnNames[i], filterValues[i]));
            }

            return result;
        }

        public IList<Expression<Func<TicketVerification, bool>>> GetGlobalSearchExpressions(string searchValue)
        {
            return new List<Expression<Func<TicketVerification, bool>>>
            {
                (t => t.Ticket.Id.ToString().StartsWith(searchValue)),
                (t => t.VerificationUTCDate.Date.ToString().Contains(searchValue)),
                (t => t.Station.Name.StartsWith(searchValue)),
                (t => t.Transport.VehicleNumber.StartsWith(searchValue))
            };
        }

        public IDictionary<string, Expression<Func<TicketVerification, object>>> GetSortExpressions()
        {
            return new Dictionary<string, Expression<Func<TicketVerification, object>>>
            {
                { "ticket", (t => t.Ticket.Id) },
                { "verificationUTCDate", (t => t.VerificationUTCDate.Date) },
                { "station", (t => t.Station.Name) },
                { "transport", (t => t.Transport.VehicleNumber) },
                { "isVerified", (t => t.IsVerified) }
            };
        }
    }
}
