using BugTrackerBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerBD.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projHelper = new ProjectHelper();
        private UserRolesHelper rolesHelper = new UserRolesHelper();

        //  A more manual way, but less efficent
        //  var myTickets = new List<Ticket>();
        //  var myProjects = projHelper.ListUserProjects(userId).ToList();
        //  foreach(var project in myProjects)
        //  {
        //      myTickets.AddRange(db.Tickets.Where(t => t.ProjectId == project.Id).ToList());
        //  }
        //  return myTickets;

        //  all this does is go out to users table in database, finds the current user, then looks at the projects they are on, then selects all the tickets on those projects, then adds those tickets to a list.

        public ICollection<Ticket> getProjTickets(string userId)
        {
            return db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).ToList();
        }

        public ICollection<Ticket> getSubTickets(string userId)
        {
           return db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
        }

        public ICollection<Ticket> getDevTickets(string userId)
        {
            return db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
        }

        public ICollection<Ticket> getAllTickets()
        {
            return db.Tickets.ToList();
        }

        public ICollection<Ticket> GetMyTickets(string userId)
        {
            var mytickets = new List<Ticket>();
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case "Admin":
                    mytickets.AddRange(db.Tickets.ToList());
                    break;
                case "ProjectManager":
                    mytickets.AddRange(getProjTickets(userId));
                    break;
                case "Developer":
                    mytickets.AddRange(db.Tickets.Where(t => t.AssignedToUserId == userId).ToList());
                    break;
                case "Submitter":
                    mytickets.AddRange(db.Tickets.Where(t => t.OwnerUserId == userId).ToList());
                    break;
            }

            return mytickets;
        }

        public ICollection<TicketNotification> GetNotifications(string userId)
        {
            var myNotifications = new List<TicketNotification>();

            myNotifications.AddRange(db.TicketNotifications.Where(n => n.RecipientId == userId).Where(n => n.Notified != true).ToList());

            return myNotifications;
        }

    }
}