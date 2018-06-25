using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTrackerBD.Models;

namespace BugTrackerBD.Models
{
    public class DashboardVM
    {
        //need to hold count data
        public TicketDashboardData TicketData { get; set; }

        public ProjectDashboardData ProjectData { get; set; }

        public TableDashboardData TableData { get; set; }

        public DashboardVM()
        {
            TableData = new TableDashboardData();
            TicketData = new TicketDashboardData();
            ProjectData = new ProjectDashboardData();
        }
    }

    public class ProjectDashboardData
    {
        public int ProjCount { get; set; }
    }

    public class TicketDashboardData
    {
        public int TicketCount { get; set; }
        public int UnAssignedTicketCount { get; set; }
        public int InProgressTicketCount { get; set; }
        public int OnHoldTicketCount { get; set; }
        public int CompletedTicketCount { get; set; }
        public int TicketNotificationCount { get; set; }
        public int TicketAttachmentCount { get; set; }
        public int TicketCommentCount { get; set; }
        public int TicketHistoryCount { get; set; }
    }

    public class TableDashboardData
    {
        public List<Projects> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<TicketNotification> TicketNotifications { get; set; }
        public List<TicketAttachment> TicketAttachments { get; set; }
        public List<TicketComment> TicketComments { get; set; }
        public List<TicketHistory> TicketHistories { get; set; }

        public TableDashboardData()
        {
            this.Tickets = new List<Ticket>();
            this.Projects = new List<Projects>();
            this.TicketNotifications = new List<TicketNotification>();
            this.TicketAttachments = new List<TicketAttachment>();
            this.TicketComments = new List<TicketComment>();
            this.TicketHistories = new List<TicketHistory>();
        }

    }

}