using BugTrackerBD.Helpers;
using BugTrackerBD.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BugTrackerBD.ActionFilters
{
    public class TicketAuthorization : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper = new UserRolesHelper();
        private ProjectHelper projHelper = new ProjectHelper();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //Maybe later change it so it redirects to an error page instead of login?

            var ticketId = filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id").Value;
            var ticket = db.Tickets.Find(ticketId);
            string userId = HttpContext.Current.User.Identity.GetUserId();
            if(ticket == null || userId == null)
            {

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
            }
            else if (userId != null && ticket != null)
            {
                var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
                var isOnProj = projHelper.IsUserOnProject(userId, ticket.ProjectsId);
                if ((myRole == "ProjectManager" && isOnProj == false))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
                }
                if ((myRole == "Developer" && ticket.AssignedToUserId != userId) ||
                    (myRole == "Submitter" && ticket.OwnerUserId != userId))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}