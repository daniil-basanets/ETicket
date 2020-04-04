
using System.Collections.Generic;

using ETicket.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicketAdmin.Controllers
{
    public class RoleController : Controller
    {
        ETicketData eTicketData;
        public RoleController(ETicketDataContext context)
        {
            //var options = new DbContextOptions<ETicketDataContext>();
            //var helper = new DbContextOptionsBuilder();
            //options = helper.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ETicket;Trusted_Connection=True;MultipleActiveResultSets=true");


            eTicketData = new ETicketData(context);
        }
        public IActionResult Index()
        {
            //List<string> names = new List<string>();
            var roles = eTicketData.Roles.GetAll();

            return View();
        }

        
    }
}