using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.WebAPI.Models
{
    public class Page<T>
    {
        public Page(int totalRowsCount, IList<T> rows)
        {
            TotalRowsCount = totalRowsCount;
            Rows = rows;
        }

        public int TotalRowsCount { get; }
        public IList<T> Rows { get; }
    }
}
