using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerBD.Models
{
    public class Projects
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Projects()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }
    }
}