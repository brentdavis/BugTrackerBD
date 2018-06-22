using BugTrackerBD.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace BugTrackerBD.Extension_Methods
{
    public static class TicketExtension
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void RecordHistory(this Ticket ticket, Ticket oldTicket)
        {
            //We have a key in webconfig that is a list of all the properties we want to track changes on, so we get it here
            var propertyList = WebConfigurationManager.AppSettings["TicketProperties"].Split(',');

            //Now we loop over every property of the ticket type
            foreach (PropertyInfo property in ticket.GetType().GetProperties())
            {
                //if this property is one i dont care about then skip it and go to next one
                if (!propertyList.Contains(property.Name))
                    continue;

                //Now we record the current value of the property and what it used to be
                var newValue = property.GetValue(ticket, null) ?? "";
                var oldValue = property.GetValue(oldTicket, null) ?? "";

                //If both the new and old vlaues are null we have nothing to record, otherwise we save the changes
                if (newValue.ToString() != oldValue.ToString())
                {
                    var ticketHistory = new TicketHistory
                    {
                        DateChanged = DateTime.Now,
                        Property = property.Name,
                        NewValue = GetValueFromKey(property.Name, newValue),
                        OldValue = GetValueFromKey(property.Name, oldValue),
                        TicketId = ticket.Id,
                        UserId = HttpContext.Current.User.Identity.GetUserId()
                    };

                    db.TicketHistories.Add(ticketHistory);
                }
            }
            db.SaveChanges();
        }

        //This code checks all the properties and gets the name instead of the id, so the user will have more relevant data (See type change from bug to request instead of 1 to 2, etc.)
        private static string GetValueFromKey(string keyName, object key)
        {
            var returnValue = "";
            if (key.ToString() == string.Empty)
                return returnValue;

            switch (keyName)
            {
                case "ProjectId":
                    returnValue = db.Projects.Find(key).Name;
                    break;
                case "TicketTypeId":
                    returnValue = db.TicketTypes.Find(key).Name;
                    break;
                case "TicketPriorityId":
                    returnValue = db.TicketPriorities.Find(key).Name;
                    break;
                case "TicketStatusId":
                    returnValue = db.TicketStatuses.Find(key).Name;
                    break;
                case "OwnerUserId":
                case "AssignedToUserId":
                    returnValue = db.Users.Find(key).DisplayName;
                    break;
                default:
                    returnValue = key.ToString();
                    break;
            }
            return returnValue;
        }

        public static async Task SendNotification(this Ticket ticket, Ticket oldTicket)
        {
            //make notification for being assigned to a new ticket

            var newAssignment = (ticket.AssignedToUserId != null && oldTicket.AssignedToUserId == null);
            var unAssignment = (ticket.AssignedToUserId == null && oldTicket.AssignedToUserId == null);
            var reAssignment = ((ticket.AssignedToUserId != null && oldTicket.AssignedToUserId != null) &&
                                (ticket.AssignedToUserId != oldTicket.AssignedToUserId));

            var body = new StringBuilder();
            body.AppendFormat("<p>Email From: <bold>{0}</bold>({1})</p>", "Administrator", WebConfigurationManager.AppSettings["emailfrom"]);
            body.AppendLine("<br/><p><u><b>Message:</b></u></p>");
            body.AppendFormat("<p><b>Project Name:</b> {0}</p>", db.Projects.FirstOrDefault(p => p.Id == ticket.ProjectsId).Name);
            body.AppendFormat("<p><b>Ticket Title:</b> {0} | Id: {1}</p>", ticket.Title, ticket.Id);
            body.AppendFormat("<p><b>Ticket Created:</b> {0}</p>", ticket.Created);
            body.AppendFormat("<p><b>Ticket Type:</b> {0}</p>", db.TicketTypes.Find(ticket.TicketTypeId).Name);
            body.AppendFormat("<p><b>Ticket Status:</b>{0}</p>", db.TicketStatuses.Find(ticket.TicketStatusId).Name);
            body.AppendFormat("<p><b>Ticket Priority:</b> {0}</p>", db.TicketPriorities.Find(ticket.TicketPriorityId).Name);

            //make the email
            IdentityMessage email = null;
            if (newAssignment)
            {

                    //create email content
                    //Send a email to the new dev letting them know they have a new ticket (for when there was no dev on ticket before)
                    email = new IdentityMessage()
                    {
                        Subject = "Bug Tracer: You have been assigned to a ticket!",
                        Body = body.ToString(),
                        Destination = db.Users.Find(ticket.AssignedToUserId).Email
                    };

                    var svc = new EmailService();
                    await svc.SendAsync(email);
            }
            else if(unAssignment)
            {
                //Send a email to the old dev letting them know they have been taken off the ticket
                email = new IdentityMessage()
                {
                    Subject = "Bug Tracer: You have been taken off a ticket!",
                    Body = body.ToString(),
                    Destination = db.Users.Find(oldTicket.AssignedToUserId).Email
                };

                var svc = new EmailService();
                await svc.SendAsync(email);
            }
            else if(reAssignment)
            {
                //Send a email to the new dev letting them know they have a new ticket (for when there was a dev on the ticket before)
                email = new IdentityMessage()
                {
                    Subject = "Bug Tracer: A ticket has been assigned to you!",
                    Body = body.ToString(),
                    Destination = db.Users.Find(ticket.AssignedToUserId).Email
                };

                var svc = new EmailService();
                await svc.SendAsync(email);

                //Send a email to the old dev letting them know they have been taken off the ticket
                email = new IdentityMessage()
                {
                    Subject = "Bug Tracer: You have been taken off of a ticket!",
                    Body = body.ToString(),
                    Destination = db.Users.Find(oldTicket.AssignedToUserId).Email
                };

            svc = new EmailService();
            await svc.SendAsync(email);
            }

            TicketNotification notification = null;
            if (newAssignment)
            {
                notification = new TicketNotification
                {
                    Body = "Bug Tracer: A ticket has been assigned to you!<br />",
                    RecipientId = ticket.AssignedToUserId,
                    TicketId = ticket.Id
                };
                db.TicketNotifications.Add(notification);
            }
            else if(unAssignment)
            {
                notification = new TicketNotification
                {
                    Body = "Bug Tracer: You have been taken off of a ticket!<br />",
                    RecipientId = oldTicket.AssignedToUserId,
                    TicketId = ticket.Id
                };
                db.TicketNotifications.Add(notification);
            }
            else if(reAssignment)
            {
                notification = new TicketNotification
                {
                    Body = "Bug Tracer: A ticket has been assigned to you!<br />",
                    RecipientId = ticket.AssignedToUserId,
                    TicketId = ticket.Id
                };
                db.TicketNotifications.Add(notification);

                notification = new TicketNotification
                {
                    Body = "Bug Tracer: You have been taken off of a ticket!<br />",
                    RecipientId = oldTicket.AssignedToUserId,
                    TicketId = ticket.Id
                };
                db.TicketNotifications.Add(notification);
            }
            db.SaveChanges();
        }
    }
}