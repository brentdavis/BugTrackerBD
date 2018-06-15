﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerBD.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Body { get; set; }
        public string RecipientId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
    }
}