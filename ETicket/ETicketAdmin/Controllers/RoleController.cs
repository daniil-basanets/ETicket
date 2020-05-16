
using System.Collections.Generic;

using ETicket.Domain;
using ETicket.Domain.Interfaces;
using ETicket.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicketAdmin.Controllers
{
    public class RoleController : Controller
    {
        ETicketData eTicketData;

        private readonly IRepository<RoleRepository> repo;

        public RoleController(IRepository<RoleRepository> repo)
        {
            this.repo = repo;
            //var options = new DbContextOptions<ETicketDataContext>();
            //var helper = new DbContextOptionsBuilder();
            //options = helper.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ETicket;Trusted_Connection=True;MultipleActiveResultSets=true");


            //eTicketData = new ETicketData(context);
        }
        public IActionResult Index()
        {
            //List<string> names = new List<string>();
            var roles = repo.GetAll();//eTicketData.Roles.GetAll();

            return View();
        }

        
    }
}