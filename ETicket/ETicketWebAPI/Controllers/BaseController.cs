using ETicket.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.WebAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        public Page<T> GetPage<T>(int pageNumber, int pageSize, IEnumerable<T> allRows)
        {
            if(pageNumber <= 0 || pageSize <= 0)
            {
                throw new InvalidParameterException();
            }

            var totalRows = allRows.Count();
            var pageRows = allRows
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize).ToList();

            return new Page<T>(totalRows, pageRows);
        }
    }
}

