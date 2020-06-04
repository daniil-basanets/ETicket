using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.DTOs
{
    public class PageDto<T>
    {
        public PageDto(int totalRowsCount, IEnumerable<T> rows)
        {
            TotalRowsCount = totalRowsCount;
            Rows = rows;
        }

        public int TotalRowsCount { get; }
        public IEnumerable<T> Rows { get; }
    }
}
