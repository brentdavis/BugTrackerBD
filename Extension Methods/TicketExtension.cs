using BugTrackerBD.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    }
}