using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Admin.Models.RegisteredUsersViewModels
{
    public class ChangeRolesViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }

        public ChangeRolesViewModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
