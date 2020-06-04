using ETicket.ApplicationServices.DTOs;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.ApplicationServices.Extensions
{
    public static class EnumerableExtensions
    {
        public static PageDto<T> ToPage<T>(this IEnumerable<T> data, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new InvalidParameterException();
            }

            var totalRows = data.Count();
            var pageRows = data
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize).ToList();

            return new PageDto<T>(totalRows, pageRows);
        }
    }
}
