using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerBD.Models
{
    public class ProjectsViewModel
    {
        public ICollection<Projects> userProjects { get; set; }
        public ICollection<Projects> allProjects { get; set; }
    }

}