using System.Collections.Generic;

namespace ETicket.ApplicationServices.Services.PagingServices.Models
{
    public class DataTablePage<T>
    {
        public int DrawCounter { get; set; }

        public int CountRecords { get; set; }

        public int CountFiltered { get; set; }

        public IEnumerable<T> PageData { get; set; }
    }
}