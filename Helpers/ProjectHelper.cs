using BugTrackerBD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BugTrackerBD.Helpers
{
    public class ProjectHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //Checks if a paticular user is on a paticular project
        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
            return (flag);
        }

        //Returns a list of every project a paticular user is on
        public ICollection<Projects> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return (projects);
        }

        //Adds a paticular user to a paticular project
        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Projects proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        //Removes a paticular user from a paticular project
        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Projects proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);

                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        //Returns every user on a paticular project
        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }

        //Returns every user not on a paticular project
        public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }
    }
}