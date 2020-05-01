using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ETicket.Admin.Models.RegisteredUsersViewModels;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "SuperUser")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        RoleManager<IdentityRole> roleManager;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            try
            {
                return View(userManager.Users.ToList());
            }
            catch (Exception e)
            {
                log.Error(e);
                return BadRequest();
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);

                if (user != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(user);
                }
                else
                {
                    log.Warn("User was not found.");
                    return NotFound();
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                log.Error(e);
                return BadRequest();
            }  
        }

        public async Task<ActionResult> EditRoles(string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    var allRoles = roleManager.Roles.ToList();
                    ChangeRolesViewModel model = new ChangeRolesViewModel
                    {
                        UserId = user.Id,
                        UserEmail = user.Email,
                        UserRoles = userRoles,
                        AllRoles = allRoles
                    };

                    return View(model);
                }

                log.Warn("User was not found.");
                return NotFound();
            }
            catch (Exception e)
            {
                log.Error(e);
                return BadRequest();
            }           
        }

        [HttpPost]
        public async Task<IActionResult> EditRoles(string userId, List<string> roles)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    // were added
                    var addedRoles = roles.Except(userRoles);
                    // were deleted
                    var removedRoles = userRoles.Except(roles);

                    await userManager.AddToRolesAsync(user, addedRoles);

                    await userManager.RemoveFromRolesAsync(user, removedRoles);

                    return RedirectToAction("Index");
                }

                log.Warn("User was not found.");
                return NotFound();
            }
            catch (Exception e)
            {
                log.Error(e);
                return BadRequest();
            }
        }
    }
}