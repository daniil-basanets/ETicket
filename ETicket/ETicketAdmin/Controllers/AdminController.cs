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
        #region Private members

        private readonly UserManager<IdentityUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            log.Info(nameof(AdminController.Index));

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
            log.Info(nameof(AdminController.Delete));

            try
            {
                var user = await userManager.FindByIdAsync(id);

                if (user != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(user);
                }
                else
                {
                    log.Warn(nameof(AdminController.Delete) + " user is null");

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
            log.Info(nameof(AdminController.EditRoles));

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

                log.Warn(nameof(AdminController.EditRoles) + " user is null");

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
            log.Info(nameof(AdminController.EditRoles) + " POST");

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

                log.Warn(nameof(AdminController.EditRoles) + " user is null");

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