using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerBD.Models
{
    public class TicketsViewModel
    {
        public ICollection<Ticket> subTickets { get; set; }
        public ICollection<Ticket> devTickets { get; set; }
        public ICollection<Ticket> projTickets { get; set; }
        public ICollection<Ticket> allTickets { get; set; }
    }
}