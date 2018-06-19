using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTrackerBD.Helpers;
using BugTrackerBD.Models;
using Microsoft.AspNet.Identity;

namespace BugTrackerBD.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        // GET: Users
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Users/Create
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,DisplayName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }
        [Authorize(Roles = "Admin, ProjectManager")]
        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            //make a select list so we can choose a role for the user
            var occupiedRoles = rolesHelper.ListUserRoles(id).FirstOrDefault();
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name", occupiedRoles);

            //making a multiple selection list that will let us assign users to one or more projects
            var projectIds = new List<int>();
            var userProjects = projectHelper.ListUserProjects(id);
            foreach (var project in userProjects)
            {
                projectIds.Add(project.Id);
            }

            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "Name", projectIds);
            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,DisplayName,Email")] ApplicationUser applicationUser, string Roles, List<int> Projects)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Admin"))
                {
                    applicationUser.UserName = applicationUser.Email;
                    //This line tells the database every field needs to be supplied or be nullable we needed to remove it because we only allow the user to enter certain things 
                    //db.Entry(applicationUser).State = EntityState.Modified;
                    //Now these lines are telling SQL which lines we would like to modify 
                    db.Users.Attach(applicationUser);
                    //remove user from all the projects they are currently on, then add them to the selected projects in the code below. 
                    foreach (var item in projectHelper.ListUserProjects(applicationUser.Id))
                    {
                        projectHelper.RemoveUserFromProject(applicationUser.Id, item.Id);
                    }
                    //loop thorugh the list of projects selected and assign each to the user based on id using helper method
                    try
                    {
                        for (int i = 0; i < Projects.Count; i++)
                        {
                            projectHelper.AddUserToProject(applicationUser.Id, Projects[i]);
                        }
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {

                        //list user roles 
                        string userRole = rolesHelper.ListUserRoles(applicationUser.Id).FirstOrDefault().ToString();


                 
                        rolesHelper.RemoveUserFromRole(applicationUser.Id, userRole);
                    }
                    catch (Exception)
                    {

                    }

                    rolesHelper.AddUserToRole(applicationUser.Id, Roles);


                    db.Entry(applicationUser).Property(x => x.FirstName).IsModified = true;
                    db.Entry(applicationUser).Property(x => x.LastName).IsModified = true;
                    db.Entry(applicationUser).Property(x => x.DisplayName).IsModified = true;
                    db.Entry(applicationUser).Property(x => x.Email).IsModified = true;

                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
            if (User.IsInRole("ProjectManager"))
            {

                applicationUser.UserName = applicationUser.Email;
                db.Users.Attach(applicationUser);
                //remove user from all the projects they are currently on, then add them to the selected projects in the code below. 
                foreach (var item in projectHelper.ListUserProjects(applicationUser.Id))
                {
                    projectHelper.RemoveUserFromProject(applicationUser.Id, item.Id);
                }
                //loop thorugh the list of projects selected and assign each to the user based on id using helper method
                try
                {
                    for (int i = 0; i < Projects.Count; i++)
                    {
                        projectHelper.AddUserToProject(applicationUser.Id, Projects[i]);
                    }
                }
                catch (Exception)
                {

                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
            return View(applicationUser);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
