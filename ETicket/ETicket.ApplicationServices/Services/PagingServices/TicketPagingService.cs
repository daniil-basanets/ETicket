using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ETicket.ApplicationServices.Services.PagingServices
{
    public class TicketPagingService : IDataTablePagingService<Ticket>
    {
        private readonly IUnitOfWork unitOfWork;

        public TicketPagingService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Ticket> GetAll()
        {
            return unitOfWork
                    .Tickets
                    .GetAll()
                    .Include(t => t.TicketType)
                    .Include(t => t.User);
        }

        public DateTime ParseDateTime(string parseValue)
        {
            if(DateTime.TryParse(parseValue, out DateTime result))
            {
                return result;
            }
            return new DateTime();
        }

        public Expression<Func<Ticket, bool>> GetSingleFilterExpression(string columnName, string filterValue)
        {
            return columnName switch
            {
                "ticketType" => (t => t.TicketType.TypeName == filterValue),
                "createdUTCDate" => (t => t.CreatedUTCDate.Date == ParseDateTime(filterValue).Date),
                "activatedUTCDate" => (t => t.ActivatedUTCDate.Value.Date == ParseDateTime(filterValue).Date),
                "expirationUTCDate" => (t => t.ExpirationUTCDate.Value.Date == ParseDateTime(filterValue).Date),
                "user" => (t => t.User.FirstName.StartsWith(filterValue) || t.User.LastName.StartsWith(filterValue)),
                _ => (t => true)
            };

            //return columnName switch
            //{
            //    "ticketType" => (t => t.TicketType.TypeName == filterValue),
            //    ("createdUTCDate") => (t => DateTime.TryParse(filterValue, out _) || DateTime.Parse(filterValue) == t.CreatedUTCDate.Date),
            //    "activatedUTCDate" => (t => t.ActivatedUTCDate.Value.Date == DateTime.Parse(filterValue).Date),
            //    "expirationUTCDate" => (t => t.ExpirationUTCDate.Value.Date == DateTime.Parse(filterValue).Date),
            //    "user" => (t => t.User.FirstName.StartsWith(filterValue) || t.User.LastName.StartsWith(filterValue)),
            //    (string s, bool d) => (t => true)
            //};

            //return columnName switch
            //{
            //    "ticketType" => (t => t.TicketType.TypeName == filterValue),
            //    "createdUTCDate" => (t => t.CreatedUTCDate.Date == DateTime.Parse(filterValue).Date),
            //    "activatedUTCDate" => (t => t.ActivatedUTCDate.Date == DateTime.Parse(filterValue).Date),
            //    "expirationUTCDate" => (t => t.ExpirationUTCDate.Date == DateTime.Parse(filterValue).Date),
            //    "user" => (t => t.User.FirstName.StartsWith(filterValue) || t.User.LastName.StartsWith(filterValue)),
            //    _ => (t => true)
            //};

            //switch (columnName) 
            //{
            //    case "ticketType":
            //        return (t => t.TicketType.TypeName == filterValue);
            //    case "createdUTCDate":                           
            //        if(DateTime.TryParse(filterValue, out DateTime createdDate))
            //        {
            //            return (t => t.CreatedUTCDate.Date == createdDate.Date);
            //        }
            //        else
            //        {
            //            return t => false;
            //        }
            //    case "activatedUTCDate":
            //        if (DateTime.TryParse(filterValue, out DateTime activatedDate))
            //        {
            //            return (t => t.ActivatedUTCDate.Value.Date == activatedDate.Date);
            //        }
            //        else
            //        {
            //            return t => false;
            //        }
            //    case "expirationUTCDate":
            //        if (DateTime.TryParse(filterValue, out DateTime date))
            //        {
            //            return (t => t.ExpirationUTCDate.Value.Date == date.Date);
            //        }
            //        else
            //        {
            //            return t => false;
            //        }
            //    case "user":
            //        {
            //            return (t => t.User.FirstName.StartsWith(filterValue) || t.User.LastName.StartsWith(filterValue));
            //        }
            //    default:
            //        return (t => true);
            //}



            //"ticketType" => ,
            //"createdUTCDate" => (t => t.CreatedUTCDate.Date == DateTime.Parse(filterValue).Date),
            //"activatedUTCDate" => (t => t.ActivatedUTCDate.Date == DateTime.Parse(filterValue).Date),
            //"expirationUTCDate" => (t => t.ExpirationUTCDate.Date == DateTime.Parse(filterValue).Date),
            //"user" => (t => t.User.FirstName.StartsWith(filterValue) || t.User.LastName.StartsWith(filterValue)),
            //_ => (t => true)

        }

        public IList<Expression<Func<Ticket, bool>>> GetGlobalSearchExpressions(string searchValue)
        {
            return new List<Expression<Func<Ticket, bool>>>
            {
                (t => t.TicketType.TypeName.StartsWith(searchValue)),
                (t => t.CreatedUTCDate.ToString().Contains(searchValue)),
                (t => t.ActivatedUTCDate.ToString().Contains(searchValue)),
                (t => t.ExpirationUTCDate.ToString().Contains(searchValue)),
                (t => t.User.FirstName.StartsWith(searchValue)),
                (t => t.User.LastName.StartsWith(searchValue))
            };
        }

        public IList<Expression<Func<Ticket, bool>>> GetFilterExpressions(string[] columnNames, string[] filterValues)
        {
            var result = new List<Expression<Func<Ticket, bool>>>();

            for (int i = 0; i < columnNames.Length; i++)
            {
                result.Add(GetSingleFilterExpression(columnNames[i], filterValues[i]));
            }

            return result;
        }

        public IDictionary<string, Expression<Func<Ticket, object>>> GetSortExpressions()
        {
            return new Dictionary<string, Expression<Func<Ticket, object>>>
            {
                { "ticketType", (t => t.TicketType.TypeName) },
                { "createdUTCDate", (t => t.CreatedUTCDate) },
                { "activatedUTCDate", (t => t.ActivatedUTCDate) },
                { "expirationUTCDate", (t => t.ExpirationUTCDate) },
                { "user", (t => t.User.LastName) }
            };
        }
    }
}
