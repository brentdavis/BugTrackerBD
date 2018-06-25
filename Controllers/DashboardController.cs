using BugTrackerBD.Helpers;
using BugTrackerBD.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTrackerBD.Controllers
{
    public class DashboardController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper ticketHelper = new TicketHelper();

        // GET: Dashboard
        public ActionResult Index()
        {
            //Create a instance of the dashboard view model
            var data = new DashboardVM();

            //Get the data for the table
            data.TableData.Projects = db.Projects.OrderByDescending(p => p.Id).Take(5).ToList();
            data.TableData.Tickets = db.Tickets.OrderByDescending(t => t.Created).Take(5).ToList();
            data.TableData.TicketNotifications = db.TicketNotifications.OrderByDescending(t => t.Id).Take(5).ToList();
            data.TableData.TicketAttachments = db.TicketAttachments.OrderByDescending(t => t.Id).Take(5).ToList();
            data.TableData.TicketComments = db.TicketComments.OrderByDescending(t => t.Created).Take(5).ToList();
            data.TableData.TicketHistories = db.TicketHistories.OrderByDescending(t => t.DateChanged).Take(5).ToList();

            //get the tickets for the current user
            var myTickets = ticketHelper.GetMyTickets(User.Identity.GetUserId());
            data.TicketData.TicketCount = myTickets.Count();
            data.TicketData.UnAssignedTicketCount = myTickets.Where(t => t.TicketStatus.Name == "UnAssigned").Count();
            data.TicketData.InProgressTicketCount = myTickets.Where(t => t.TicketStatus.Name == "In Progress").Count();
            data.TicketData.OnHoldTicketCount = myTickets.Where(t => t.TicketStatus.Name == "On Hold").Count();
            data.TicketData.CompletedTicketCount = myTickets.Where(t => t.TicketStatus.Name == "Resolved").Count();

            data.TicketData.TicketNotificationCount = db.TicketNotifications.Count();
            data.TicketData.TicketAttachmentCount = db.TicketAttachments.Count();
            data.TicketData.TicketCommentCount = db.TicketComments.Count();
            data.TicketData.TicketHistoryCount = db.TicketHistories.Count();

            //Load up all the Project Dashboard Data
            data.ProjectData.ProjCount = db.Projects.Count();
           


            return View(data);
        }
    }
}