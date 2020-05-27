using ETicket.Admin.Models.DataTables;
using System;
using System.Collections.Generic;
using System.Text;
using ETicket.ApplicationServices.Services.PagingServices.Models;

namespace ETicket.ApplicationServices.Services.DataTable.Interfaces
{
    public interface IDataTableService<T>
    {
        DataTablePage<T> GetDataTablePage(DataTablePagingInfo pagingInfo);
    }
}
