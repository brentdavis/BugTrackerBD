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
        //public TicketDashboardData TicketData { get; set; }

        //public TableDashboardData TableData { get; set; }
       
    }

    public class ProjectCounts
    {
        public int ProjectCount { get; set; }
    }
    public class TicketCounts
    {
        public int TicketCount { get; set; }

        public int UnassignedTicketCount { get; set; }
        public int InProgressTicketCount { get; set; }
        public int OnHoldTicketCount { get; set; }
        public int CompletedTicketCount { get; set; }

        public int TicketNotificationCount { get; set; }
        public int TicketAttachCount { get; set; }
        public int TicketCommentCount { get; set; }
        public int TicketHistoryCount { get; set; }
    }

}