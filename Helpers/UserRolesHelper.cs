using BugTrackerBD.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerBD.Helpers
{
    public class UserRolesHelper
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();

        //Checks if a paticualar user is in a paticular role
        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }

        //Returns a list of roles a paticular user is in
        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }

        //Adds a paticular user to a paticular role
        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        //Removes a paticular user from a paticular role
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        //Returns a list of all users in a paticular role
        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }

            return resultList;
        }

        //Returns a list of all users not in a paticular role
        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }

            return resultList;
        }
    }
}