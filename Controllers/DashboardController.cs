using BugTrackerBD.Models;
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

        // GET: Dashboard
        public ActionResult Index()
        {

            var data = new DashboardVM();

            //data.TableData.Projects = db.Projects.OrderByDescending(p => p.Id).Take(5).ToList();
            //data.TableData.Tickets = db.Tickets.OrderByDescending(t => t.Created).Take(5).ToList();
            //data.TableData.TicketNotifications = db.TicketNotifications.OrderByDescending(t => t.Id).Take(5).ToList();
            //data.TableData.TicketAttachments = db.TicketAttachments.OrderByDescending(t => t.)
            

            return View();
        }
    }
}