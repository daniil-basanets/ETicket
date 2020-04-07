using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBContextLibrary.Domain;
using DBContextLibrary.Domain.Entities;
namespace ETicketAdmin.Controllers
{
    class PrivilegeController : Controller
    {
        #region

        private readonly ETicketDataContext context;

        #endregion

        public PrivilegeController(ETicketDataContext context)
        {
            this.context = context;
        }

        public IActionResult Index()    //output privilegies
        {
            return View(context.Privileges);
        }


    }
}
