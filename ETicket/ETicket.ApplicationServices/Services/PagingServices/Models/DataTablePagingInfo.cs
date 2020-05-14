using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Admin.Models.DataTables
{
    public class DataTablePagingInfo
    {
        //The draw counter to prevent Cross Site Scripting (XSS) attacks
        //Returning the same DrawCounter will break the request
        public int DrawCounter { get; set; }

        //For sorting
        public string SortColumnName { get; set; }
        public string SortColumnDirection { get; set; }

        //For paging
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalEntries { get; set; }

        //For a global search on all columns
        public string SearchValue { get; set; }

        //Arrays for filtering
        public string[] FilterColumnNames { get; set; }
        public string[] FilterValues { get; set; }
    }
}